using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInGame : MonoBehaviour
{
    [SerializeField] private GameObject menuGame;
    [SerializeField] private FreezeGame freezeGame;

    private void Start()
    {
        menuGame.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) 
        {
            menuGame.SetActive(true);
            freezeGame.PauseGame();
        }
    }

    public void ClosedMenu() 
    {
        menuGame.SetActive(false);
        freezeGame.ResumeGame();
    }
}
