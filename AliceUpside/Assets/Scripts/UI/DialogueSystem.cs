using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] private FreezeGame freezeGame = new FreezeGame();
    [SerializeField] private GameObject[] dialogueNPC;
    [SerializeField] private GameObject canvasNPC;
    [SerializeField] private float raycastRange;

    private Camera mainCamera;
    private RaycastHit hit;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        RaycastCheck();
    }

    private void RaycastCheck()
    {
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, raycastRange))
        {
            if (InputController.Instance.InteractAction.triggered) 
            {
                if (hit.transform.gameObject.tag == "NPC")
                {
                    var NPC = hit.transform.gameObject.GetComponent<DialogueSystem>();
                    NPC.dialogueNPC[0].SetActive(true);
                    freezeGame.PauseGame();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
            if (other.gameObject.CompareTag("Player"))
            {
                if (InputController.Instance.InteractAction.triggered)
                {
                var NPC = GetComponent<DialogueSystem>();
                NPC.dialogueNPC[0].SetActive(true);
                freezeGame.PauseGame();
                }
            }    
    }


    public void ButtonDialogue1()
    {
        int wich = 1;
        for (int i = 0; i < dialogueNPC.Length; i++)
        {
            this.dialogueNPC[i].SetActive(i == wich);
        }
    }
    public void ButtonDialogue2()
    {
        int wich = 2;
        for (int i = 0; i < dialogueNPC.Length; i++)
        {
            this.dialogueNPC[i].SetActive(i == wich);
        }
    }
    public void ButtonDialogue3()
    {
        int wich = 3;
        for (int i = 0; i < dialogueNPC.Length; i++)
        {
            this.dialogueNPC[i].SetActive(i == wich);
        }
    }
    public void ButtonDialogue4()
    {
        int wich = 4;
        for (int i = 0; i < dialogueNPC.Length; i++)
        {
            this.dialogueNPC[i].SetActive(i == wich);
        }

        freezeGame.ResumeGame();
        canvasNPC.SetActive(false);
    }
}
