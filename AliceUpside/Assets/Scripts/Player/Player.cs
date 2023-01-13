using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Cinemachine;




public class Player : MonoBehaviour
{
    #region Variaveis
    private enum AnimationPlayer
    {
        Animation_Run, Animation_RunBack, Animation_Idle, Animation_Aim
    }
    public enum StatePlayer
    {
        State_Base, State_Rifle
    }
    public StatePlayer currentState;

    [Header("Player Combat")]
    [SerializeField] private GameObject shotgun;
    [SerializeField] private CinemachineCameraOffset camOffset_Aim;
    [SerializeField] private Rig rigLayerRifle;
    private float floatCamOffset;

    [Header("Player Features")]
    private Rigidbody rb;
    private Animator anim;
    private float playerHeight = 1f;

    [Header("Movement")]
    [SerializeField] private float runSpeed;
    [SerializeField] private float walkSpeed;
    private float moveSpeed;

    [Header("Jump")]
    [SerializeField] private float jumpForce;

    [Header("Ground")]
    [SerializeField] LayerMask groundMask;
    [SerializeField] private float groundDistance = 0.4f;
    private bool isGrounded;

    [Header("Drag")]
    [SerializeField] private float groundDrag = 6f;
    [SerializeField] private float airDrag = 2f;

    private Vector3 directionPlayer;
    private Vector3 slopeMoveDirection;
    public RaycastHit slopeHit;
    private float horizontalMovement;
    private float verticalMovement;
    public bool isEquippedWeapon = false;

    public static Player Instance;
    #endregion

    #region MÉTODOS PADRÕES

    private void Awake(){
        /*
        if (Instance != null && Instance != this) { 
            Destroy(this); 
        } 
        else { 
            Instance = this; 
        } 
        */
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        shotgun.SetActive(false);
        rigLayerRifle.weight = 0;

        ControlAnimation(AnimationPlayer.Animation_Idle); 
    }

    private void Update()
    {
        //Methods Movement and Physics
        slopeMoveDirection = Vector3.ProjectOnPlane(directionPlayer, slopeHit.normal);
        PhysicsControlDrag();
        PhysicsGrounded();
        PhysicsSlope();
        InputMove();
        Run();


        //Methods Combat
        WeaponsEquiped();
        CameraFPS();

    }

    private void FixedUpdate()
    {
        MovePlayer();
    }
    #endregion

    #region MOVIMENTAÇÃO
    private void InputMove() 
    {
        horizontalMovement = Input.GetAxis("Horizontal");
        verticalMovement = Input.GetAxis("Vertical");
        directionPlayer = transform.forward * verticalMovement + transform.right * horizontalMovement;
        
        anim.SetFloat("Horizontal", horizontalMovement);
        anim.SetFloat("Vertical", verticalMovement);
    }

    private bool CheckInputMove()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void MovePlayer() 
    {
        if (CheckInputMove())
        {
            rb.AddForce(directionPlayer.normalized * moveSpeed, ForceMode.Acceleration);
            //rb.velocity = directionPlayer * moveSpeed;
        }
        if(isGrounded && PhysicsSlope())
        {
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed, ForceMode.Acceleration);
        }
    }

    
    private void Run()
    {
        if (isGrounded || PhysicsSlope())
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (verticalMovement > 0)
                {
                    ControlAnimation(AnimationPlayer.Animation_Run);
                }
                else if (verticalMovement < 0)
                {
                    ControlAnimation(AnimationPlayer.Animation_RunBack);

                }
            }
            else 
            {
                ControlAnimation(AnimationPlayer.Animation_Idle); 
            }
        }
    }
    /*
    private void Jump() 
    {
        if(isGrounded) 
        {
            anim.SetBool("Jump", false);
            if (Input.GetKey(KeyCode.Space)) 
            {
                rb.velocity = Vector3.up * jumpForce;
            }
        }
        else 
        {
            anim.SetBool("Jump", true);
        }
    }*/
    #endregion

    #region COMBATE
    void WeaponsEquiped() 
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            ControlState(StatePlayer.State_Rifle);
            isEquippedWeapon = true;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            ControlState(StatePlayer.State_Base);
            isEquippedWeapon = false;
        }
    }

    private void CameraFPS() 
    {
        if (isEquippedWeapon) 
        {
            //
            camOffset_Aim.m_Offset.z = Mathf.Lerp(camOffset_Aim.m_Offset.z, floatCamOffset, Time.deltaTime * 5);
        }
        switch (Input.GetButton("Fire2")) 
        {
            case true:
                floatCamOffset = 1.7f;
                break;
            case false:
                floatCamOffset = 0;
                break;
        }
    }
    #endregion

    #region CONTROLADORES
    private void ControlAnimation(AnimationPlayer animationPlayer)
    {
        switch (animationPlayer)
        {
            case AnimationPlayer.Animation_Run:
                moveSpeed = runSpeed;
                anim.SetBool("Run", true);
                anim.SetBool("Run Back", false);
                break;

            case AnimationPlayer.Animation_RunBack:
                moveSpeed = runSpeed;
                anim.SetBool("Run", false);
                anim.SetBool("Run Back", true);
                break;

            case AnimationPlayer.Animation_Idle:
                moveSpeed = walkSpeed;
                anim.SetBool("Run", false);
                anim.SetBool("Run Back", false);
                break;

            case AnimationPlayer.Animation_Aim:
                moveSpeed = walkSpeed;
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
                shotgun.SetActive(false);
                break;

            case StatePlayer.State_Rifle:
                anim.SetLayerWeight(anim.GetLayerIndex("State Rifle"), 1);
                rigLayerRifle.weight = 1;
                shotgun.SetActive(true);
                break;
        }
    }
    #endregion

    #region FISICAS
    private void PhysicsGrounded() 
    {
        isGrounded = Physics.CheckSphere(transform.position, groundDistance, groundMask);
    }

    private void PhysicsControlDrag() 
    {
        if (isGrounded) 
        {
            rb.drag = groundDrag;
        }
        else 
        {
            rb.drag = airDrag;
        }
    }

    private bool PhysicsSlope() 
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight/2 + 0.5f)) 
        {
           if(slopeHit.normal != Vector3.up) 
           {
                return true;
           }
           else 
           {
                return false;
           }
        }
        return false;
    }
    #endregion
}
