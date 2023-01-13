using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Experimental.GlobalIllumination;


public class CycleDayNight : MonoBehaviour, IDataPersisting
{
    //1 DIA = 24 HORAS
    //1 HORA = 60 MINUTOS
    //1 MINUTO = 60 SEGUNDOS
    //1 DIA TOTAL = 86400 SEGUNDOS
    
    [Header("Config CycleDayNight")]
    [SerializeField] private Transform directionalLight;
    [SerializeField] private Light sun;
    [SerializeField] private int timeDaySeconds;
    private int dayCount;
    private float seconds;
    private float multiplier;
    private bool isNight;

    [Header("Config Lights")]
    [SerializeField] private GameObject[] lightsGame;
    [SerializeField] private GameObject lightFireCampEnd;


    [Header("Config Temperature")]
    [SerializeField] private float damageTemperatureWorld = 5f;
    private float temperatureWorld;
    private float temperatureWorldMax = 15f;

    [Header("HUD Day")]
    [SerializeField] private TextMeshProUGUI dayText;

    //Encapsulados
    public float TemperatureWorld { get => temperatureWorld; }
    public float TemperatureWorldMax { get => temperatureWorldMax; }

    void Start()
    {
        temperatureWorld = temperatureWorldMax;
        multiplier = 86400 / timeDaySeconds;
    }

    void Update()
    {
        dayText.text = dayCount.ToString();

        ProcessingSky();
        CalculateHour();
        ControlTemperatureWorld();
        DamageTemperatureWorld();
        ControlLightsGame();
    }

    public void LoadData(GameData data) 
    {
        this.dayCount = data.dayCountData;
    }

    public void SaveData(ref GameData data)
    {
        data.dayCountData = this.dayCount;
    }

    private void CalculateHour() 
    {
        seconds += Time.deltaTime * multiplier;
        if (seconds >= 86400)
        {
            seconds = 0;
            dayCount++;
        }

        if (seconds >= 86400 / 2)
        {
            isNight = true;
            sun.intensity -= Time.deltaTime;
        }
        else
        {
            isNight = false;
            sun.intensity = 1;
        }
    }

    private void ProcessingSky() 
    {
        /* 00h = -90º 
         * 06h = 0º
         * 12h = 90º
         * 18h = 180º
         * 23h59 = 270º
         */

        float rotationX = Mathf.Lerp(0, 360, seconds / 86400);
        directionalLight.rotation = Quaternion.Euler(rotationX, 0, 0);
    }

    private void ControlTemperatureWorld() 
    {
        //Decrease temperature
        if (isNight) 
        {
            temperatureWorld -= Time.deltaTime;
        }
        else
        {
            temperatureWorld += Time.deltaTime;
        }
        
        //Formalizando valores infinitos
        if(temperatureWorld >= temperatureWorldMax) 
        {
            temperatureWorld = temperatureWorldMax;
        }
        else if(temperatureWorld <= 0)
        {
            temperatureWorld = 0;
        }
    }

    private void DamageTemperatureWorld() 
    {
        //Dano no player com a temperatura
        if (temperatureWorld <= 5 && !PlayerController.Instance.IsAroundHotAmbient)
        {
            PlayerController.Instance.TakeDamage(damageTemperatureWorld * Time.deltaTime);
            PlayerController.Instance.IsTakeDamage = true;
        }
        else
        {
            PlayerController.Instance.IsTakeDamage = false;
        }
    }

    private void ControlLightsGame() 
    {
        if(isNight == true) 
        {
            lightFireCampEnd.SetActive(false);
            for (int i = 0; i < lightsGame.Length; i++)
            {
                lightsGame[i].SetActive(true);
            }
        }
        else
        {
            lightFireCampEnd.SetActive(true);
            for (int i = 0; i < lightsGame.Length; i++)
            {
                lightsGame[i].SetActive(false);

            }
        }
    }
}
