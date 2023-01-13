using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    //Data Manager Day
    public int dayCountData;

    //Data Status Player
    public Vector3 playerPositionData;
    public int seedCountData;
    public int woodCountData;
    public float healthPlayerData;
    public float staminePlayerData;

    //Data Status Enemies
    public float healthMonsterData;

    //public SerializableDictionary<string, bool> plantCollected;


    //Valores padrão assim que iniciar o jogo sem um carregamento de dados (Load Data).
    public GameData() 
    {
        this.dayCountData = 0;
        this.seedCountData = 10;
        this.woodCountData = 0;
        this.healthPlayerData = 100f;
        this.staminePlayerData = 70f;
        this.healthMonsterData = 50f;
        playerPositionData = new Vector3(41, 1.05f, -36.05f);
        //plantCollected = new SerializableDictionary<string, bool>();
    }
}
