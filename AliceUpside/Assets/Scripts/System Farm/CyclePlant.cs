using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CyclePlant : MonoBehaviour
{
    [SerializeField] private string id;

    [ContextMenu("Gerar guia para id")]
    private void GenerateGuid() 
    {
        id = System.Guid.NewGuid().ToString();
    }

    public enum PlantStatus
    {
        Phase_Seed, Phase_Sprout, Phase_Haverst
    }
    public PlantStatus plantStatus;

    [SerializeField] GameObject[] plantPhases;
    [SerializeField] float finalSeedTime;
    [SerializeField] float finalSprouTime;
    [SerializeField] float finalHaverstTime;

    private float currentSeedTime;
    private float currentSprouTime;
    private float currentHaverstTime;
    private bool harverstIsReady;

    public bool HarverstIsReady { get => harverstIsReady; set => harverstIsReady = value; }

    private void Awake()
    {
        CheckLandStatus(PlantStatus.Phase_Seed);
    }

    private void Start()
    {
        GenerateGuid();
        harverstIsReady = false;
    }

    private void Update()
    {
        CheckLandStatus(plantStatus);
        SwitchLandStatus();
    }

    private void SwitchLandStatus()
    {
        if (currentSeedTime >= finalSeedTime)
        {
            CheckLandStatus(PlantStatus.Phase_Sprout);
            currentSeedTime = 0;
        }
        if (currentSprouTime >= finalSprouTime)
        {
            CheckLandStatus(PlantStatus.Phase_Haverst);
            currentSprouTime = 0;
        }
        if (currentHaverstTime >= finalHaverstTime)
        {
            currentHaverstTime = 0;
        }
    }
    private void CheckLandStatus(PlantStatus statusToSwitch)
    {
        plantStatus = statusToSwitch;

        switch (statusToSwitch)
        {
            case PlantStatus.Phase_Seed:
                currentSeedTime += Time.deltaTime;
                plantPhases[0].SetActive(true);
                plantPhases[1].SetActive(false);
                plantPhases[2].SetActive(false);
                break;

            case PlantStatus.Phase_Sprout:
                currentSprouTime += Time.deltaTime;
                plantPhases[0].SetActive(false);
                plantPhases[1].SetActive(true);
                plantPhases[2].SetActive(false);
                break;

            case PlantStatus.Phase_Haverst:
                currentHaverstTime += Time.deltaTime;
                harverstIsReady = true;
                plantPhases[0].SetActive(false);
                plantPhases[1].SetActive(false);
                plantPhases[2].SetActive(true);
                break;
        }
    }
}
