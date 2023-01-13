using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class UIController : MonoBehaviour //IDataPersisting
{
    #region VARIAVÉIS
    public static UIController instance;

    [Header("HUD Player")]
    [SerializeField] private PlayerController player;
    [SerializeField] private FreezeGame freezeGame;
    [SerializeField] private Slider sliderStamine;
    [SerializeField] private Slider sliderHealthPlayer;

    [Header("HUD Temperature")]
    [SerializeField] private CycleDayNight cycleDayNight;
    [SerializeField] private Slider sliderTemperatureWorld;
    [SerializeField] private Image fillTemperatureWorld;

    [Header("HUD Geral")]
    [SerializeField] private TextMeshProUGUI quantifyWoodText;
    [SerializeField] private TextMeshProUGUI quantifySeedText;
    //[SerializeField] private Text quantifyFoodText;

    [SerializeField] private GameObject recursosUI;
    [SerializeField] private Animation recursosAnim;
    bool animRecursosInReproduction;

    [Header("HUD Weapons")]
    [SerializeField] private GameObject[] borderSelectionSlot;

    //[SerializeField] private GameObject canvasMenu;
    #endregion

    #region METÓDOS PADRÕES
    private void Awake()
    {
        for (int i = 0; i < 3; i++)
        {
            borderSelectionSlot[i].SetActive(false);
        }

        sliderStamine.maxValue = player.StaminePlayerMax;
        sliderHealthPlayer.maxValue = player.HealthPlayerMax;
        sliderTemperatureWorld.maxValue = cycleDayNight.TemperatureWorldMax;
    }

    /*
    public void LoadData(GameData data)
    {
        foreach (KeyValuePair<string, bool> pair in data.plantCollected)
        {
            if (pair.Value) 
            {

            }
        }
    }
    */
    public void SaveData(ref GameData data)
    {
        //No necessary add data for while.
    }

    private void Update() 
    {
        //EnableSettings();
        SliderControlHealthPlayer();
        SliderControlStamine();
        SliderControlTemperature();

        ControlValueWood();
        ControlValueSeed();

        BorderSelection1();
        BorderSelection2();
        BorderSelection3();

        if (player.IsDead) 
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKey(KeyCode.I) && animRecursosInReproduction != true) 
        {
            StartCoroutine(RecursosAnimationPopUp());
        }
    }
    #endregion

    #region CONTROLADORES UI IMAGE
    public void SliderControlHealthPlayer() 
    {
        sliderHealthPlayer.value = player.HealthPlayer;
    }

    public void SliderControlStamine()
    {
        sliderStamine.value = player.StaminePlayer;
    }

    public void SliderControlTemperature()
    {
        sliderTemperatureWorld.value = cycleDayNight.TemperatureWorld;
    }

    private void ControlValueWood() 
    {
        quantifyWoodText.text = player.QuantityWood.ToString();
    }

    private void ControlValueSeed()
    {
        quantifySeedText.text = player.QuantitySeed.ToString();
    }

    private void BorderSelection1() 
    {
        if (InputController.Instance.Item1Action.triggered) 
        {
            borderSelectionSlot[0].SetActive(true);
            borderSelectionSlot[1].SetActive(false);
            borderSelectionSlot[2].SetActive(false);
        }
    }
    private void BorderSelection2()
    {
        if (InputController.Instance.Item2Action.triggered)
        {
            borderSelectionSlot[0].SetActive(false);
            borderSelectionSlot[1].SetActive(true);
            borderSelectionSlot[2].SetActive(false);
        }
    }
    private void BorderSelection3()
    {
        if (InputController.Instance.Item3Action.triggered)
        {
            borderSelectionSlot[0].SetActive(false);
            borderSelectionSlot[1].SetActive(false);
            borderSelectionSlot[2].SetActive(true);
        }
    }
    #endregion

    IEnumerator RecursosAnimationPopUp() 
    {
        recursosUI.SetActive(true);
        recursosAnim.Play("Recursos Animation");
        animRecursosInReproduction = true;
        yield return new WaitForSeconds(5f);
        animRecursosInReproduction = false;
        recursosUI.SetActive(false);
    }

    #region OpenSettings
    /*public void EnableSettings(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            canvasMenu.SetActive(!canvasMenu.activeSelf);
            canvasDisable.SetActive(false);
            //Desabilitar Camera
            cam.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
        }else if(!canvasMenu.activeSelf){
            cam.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }*/
    #endregion
}
