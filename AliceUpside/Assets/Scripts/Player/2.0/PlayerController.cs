using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour, IDataPersisting
{
    #region VARIAVÉIS
    private enum AnimationPlayer
    {
        Animation_Run, Animation_RunBack, Animation_Idle, Animation_Aim
    }
    public enum StatePlayer
    {
        State_Base, State_Rifle, State_Pistol
    }
    public StatePlayer currentState;

    public static PlayerController Instance;

    [Header("Movement")]
    [SerializeField] private float runSpeed = 6.0f;
    [SerializeField] private float walkSpeed = 2.0f;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float cameraSensibility = 5f;
    private Transform cameraTransform;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private bool isRun;
    private bool canRun;

    [Header("Status Items")]
    private int quantitySeed;
    private int quantityWood;
    private int quantityWoodMax = 15;
    private int quantitySeedMax = 30;

    [Header("Combat Weapons")]
    [SerializeField] private GameObject rifle;
    [SerializeField] private GameObject pistol;
    [SerializeField] private Rig rigLayerRifle;
    [SerializeField] private Rig rigLayerPistol;

    [Header("Status Player")]
    [SerializeField] AudioSource audioWalk;
    [SerializeField] AudioSource audioRun;
    [SerializeField] AudioSource audioBreath;
    [SerializeField] private float healthPlayerMax = 100f;
    [SerializeField] private float staminePlayerMax = 50f;
    public float healthPlayer;
    private float staminePlayer;
    private float healthRegenate = 2f;
    private float stamineRegenate = 2f;
    private bool isTakeDamage;
    private bool isAroundHotAmbient;
    private bool isDead;


    private Animator anim;
    private CharacterController controller;

    //ENCAPSULADOS
    public float HealthPlayer { get => healthPlayer; set => healthPlayer = value; }
    public float StaminePlayer { get => staminePlayer; set => staminePlayer = value; }
    public float StaminePlayerMax { get => staminePlayerMax; }
    public float HealthPlayerMax { get => healthPlayerMax; }
    public int QuantitySeed { get => quantitySeed; set => quantitySeed = value; }
    public bool IsTakeDamage { get => isTakeDamage; set => isTakeDamage = value; }
    public bool IsAroundHotAmbient { get => isAroundHotAmbient; set => isAroundHotAmbient = value; }
    public int QuantityWood { get => quantityWood; set => quantityWood = value; }
    public bool IsDead { get => isDead; }
    #endregion

    #region METÓDOS PADRÕES
    private void Awake()
    {
        if(Instance == null) 
        {
            Instance = this;
        }

        
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
    }

    public void LoadData(GameData data) 
    {
        this.transform.position = data.playerPositionData;
        this.HealthPlayer = data.healthPlayerData;
        this.StaminePlayer = data.staminePlayerData;
        this.QuantitySeed = data.seedCountData;
        this.QuantityWood = data.woodCountData;     
    }

    public void SaveData(ref GameData data)
    {
        data.playerPositionData = this.transform.position;
        data.healthPlayerData = this.HealthPlayer;
        data.staminePlayerData = this.StaminePlayer;
        data.seedCountData = this.QuantitySeed;
        data.woodCountData = this.QuantityWood;
    }

    private void OnEnable()
    {
        healthPlayer = healthPlayerMax;
        staminePlayer = staminePlayerMax;
        quantitySeed = 10;

        ControlState(StatePlayer.State_Base);
        InputController.Instance.Item1Action.performed += keyboard_1 => EquipItem1();
        InputController.Instance.Item2Action.performed += keyboard_2 => EquipItem2();
        InputController.Instance.Item3Action.performed += keyboard_3 => EquipItem3();
    }

    private void OnDisable()
    {
        InputController.Instance.Item1Action.performed -= keyboard_1 => EquipItem1();
        InputController.Instance.Item2Action.performed -= keyboard_2 => EquipItem2();
        InputController.Instance.Item3Action.performed -= keyboard_3 => EquipItem3();
    }

    void Update()
    {
        PlayerMove();
        PlayerRotate();
        PlayerJump();
        CheckGround();
        Run();
        IncreaseHealthPlayer();
        IncreaseStaminePlayer();

        if (healthPlayer <= 0)
        {
            Dead();
        }
        if (healthPlayer <= 0) 
        {
            healthPlayer = 0;
        }
        if(staminePlayer > 10) 
        {
            canRun = true;
        }
        if(staminePlayer <= 0) 
        {
            staminePlayer = 0;
            isRun = false;
            canRun = false;
        }
        if(staminePlayer < 10) 
        {
            if(audioBreath.isPlaying == false) 
            {
                audioBreath.Play();
            }
        }
        else 
        {
            if (audioBreath.isPlaying == true)
            {
                audioBreath.Stop();
            }
        }

        if (isRun) 
        {
            if (audioRun.isPlaying == false)
            {
                audioRun.Play();
            }
        }
        else
        {
            if (audioRun.isPlaying == true)
            {
                audioRun.Stop();
            }
        }
    }
    #endregion

    void PlayerMove() 
    {
        Vector3 move = new Vector3(InputController.Instance.InputMove.x, 0, InputController.Instance.InputMove.y);
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0;

        if(move.magnitude > 0) 
        {
            if (audioWalk.isPlaying == false)
            {
                audioWalk.Play();
            }
        }
        else 
        {
            if (audioWalk.isPlaying == true)
            {
                audioWalk.Stop();
            }
        }
        controller.Move(move * Time.deltaTime * playerSpeed);
        anim.SetFloat("Horizontal", InputController.Instance.InputMove.x);
        anim.SetFloat("Vertical", InputController.Instance.InputMove.y);
    }

    void PlayerRotate() 
    {
        Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, cameraSensibility * Time.deltaTime);
    }

    private void Run()
    {
        if (canRun && playerVelocity.y <= 1 && InputController.Instance.SprintAction.IsPressed())
        {
            isRun = true;
            if (InputController.Instance.InputMove.y > 0)
            {
                DecreaseStaminePlayer();
                ControlAnimation(AnimationPlayer.Animation_Run);
            }
            else if (InputController.Instance.InputMove.y < 0)
            {
                DecreaseStaminePlayer();
                ControlAnimation(AnimationPlayer.Animation_RunBack);
            }
        }
        else
        {
            ControlAnimation(AnimationPlayer.Animation_Idle);
            isRun = false;
        }
    }

    void PlayerJump() 
    {
        if (groundedPlayer && InputController.Instance.JumpAction.triggered)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void CheckGround() 
    {
        groundedPlayer = controller.isGrounded;

        if (groundedPlayer && playerVelocity.y <= 1)
        {
            playerVelocity.y = 0f;
        }
    }
    #region GERENCIAMENTO DO FARM E COLETA
    public void IncreaseAmountSeeds()
    {
        if(quantitySeed < quantitySeedMax) 
        {
            quantitySeed++;
        }
        else if(quantitySeed >= quantitySeedMax)
        {
            quantitySeed = quantitySeedMax;
        }
    }

    public void DecreaseAmountSeeds()
    {
        quantitySeed--;
    }

    public void IncreaseAmountWood() 
    {
        if (quantityWood < quantityWoodMax)
        {
            quantityWood ++;
        }
        else if(quantityWood >= quantityWoodMax)
        {
            quantityWood = quantityWoodMax;
        }      

    }

    public void DecreaseAmountWood()
    {
        if(quantityWood > 0) 
        {
            quantityWood--;
        }
    }

    #endregion

    #region COMBATE
    public void TakeDamage(float damage) 
    {
        healthPlayer -= damage;
    }

    private void IncreaseHealthPlayer()
    {
        if (isTakeDamage != true && healthPlayer < healthPlayerMax)
        {
            StartCoroutine(HealthRegenerateDelay());
        }
    }

    private void DecreaseStaminePlayer()
    {
        staminePlayer -= Time.deltaTime * stamineRegenate * 2;
    }

    private void IncreaseStaminePlayer() 
    {
        if (isRun != true && staminePlayer < staminePlayerMax)
        {
            StartCoroutine(StamineRegenerateDelay());
        }
    }

    private void Dead() 
    {
        isDead = true;
        Destroy(gameObject, 0.5f);
    }

    IEnumerator StamineRegenerateDelay() 
    {
        yield return new WaitForSeconds(2.5f);
        staminePlayer += stamineRegenate * Time.deltaTime;
    }
    IEnumerator HealthRegenerateDelay()
    {
        yield return new WaitForSeconds(2f);
        healthPlayer += healthRegenate * Time.deltaTime;
    }
    #endregion

    #region CONTROLADORES
    private void EquipItem1()
    {
        ControlState(StatePlayer.State_Rifle);
    }

    private void EquipItem2()
    {
        ControlState(StatePlayer.State_Pistol);
    }

    private void EquipItem3()
    {
        ControlState(StatePlayer.State_Base);
    }

    private void ControlAnimation(AnimationPlayer animationPlayer)
    {
        switch (animationPlayer)
        {
            case AnimationPlayer.Animation_Run:
                playerSpeed = runSpeed;
                anim.SetBool("Run", true);
                anim.SetBool("Run Back", false);
                break;

            case AnimationPlayer.Animation_RunBack:
                playerSpeed = runSpeed;
                anim.SetBool("Run", false);
                anim.SetBool("Run Back", true);
                break;

            case AnimationPlayer.Animation_Idle:
                playerSpeed = walkSpeed;
                anim.SetBool("Run", false);
                anim.SetBool("Run Back", false);
                break;

            case AnimationPlayer.Animation_Aim:
                playerSpeed = walkSpeed;
                anim.SetBool("Run", false);
                anim.SetBool("Run Back", false);
                break;
        }
    }

    private void ControlState(StatePlayer statePlayer)
    {
        switch (statePlayer)
        {
            case StatePlayer.State_Base:
                anim.SetLayerWeight(anim.GetLayerIndex("State Rifle"), 0);
                rigLayerRifle.weight = 0;
                rigLayerPistol.weight = 0;
                rifle.SetActive(false);
                pistol.SetActive(false);
                break;

            case StatePlayer.State_Rifle:
                anim.SetLayerWeight(anim.GetLayerIndex("State Rifle"), 1);
                rigLayerRifle.weight = 1;
                rigLayerPistol.weight = 0;
                rifle.SetActive(true);
                pistol.SetActive(false);
                break;

            case StatePlayer.State_Pistol:
                anim.SetLayerWeight(anim.GetLayerIndex("State Rifle"), 1);
                rigLayerRifle.weight = 0;
                rigLayerPistol.weight = 1;
                rifle.SetActive(false);
                pistol.SetActive(true);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "AmbientHot") 
        {
            isAroundHotAmbient = true;
        }
        if (other.gameObject.name == "Trigger Outside")
        {
            var door = other.gameObject.GetComponentInParent<Animation>();
            door.Play("Door Outside");
        }
        else if(other.gameObject.name == "Trigger Inside")
        {
            var door = other.gameObject.GetComponentInParent<Animation>();
            door.Play("Door Inside");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "AmbientHot")
        {
            isAroundHotAmbient = false;
        }
    }
    #endregion
}