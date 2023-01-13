using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistingManager : MonoBehaviour
{
    [Header("Gerenciador de Armazenamento de Arquivos")]
    [SerializeField] private string fileName;
    //[SerializeField] private bool useEncryption;

    private GameData gameData;
    private List<IDataPersisting> dataPersistingObjects;
    private FileDataHandler dataHandler;

    public static DataPersistingManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null) 
        {
            Debug.LogError("Foi encontrado mais de um gerenciador de dados persistente na cena");
        }
        Instance = this;
    }

    private void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);//, useEncryption);
        this.dataPersistingObjects = FindAllDataPersistingObjects();
        LoadGame();
    }

    public void NewGame() 
    {
        this.gameData = new GameData();
    }
    public void LoadGame()
    {
        this.gameData = dataHandler.Load();

        if(this.gameData == null) 
        {
            Debug.Log("Dados não encontrado. Iniciando novos Dados");
            NewGame();
        }

        foreach (IDataPersisting dataPersistingObj in dataPersistingObjects)
        {
            dataPersistingObj.LoadData(gameData);
        }
    }
    public void SaveGame()
    {
        foreach (IDataPersisting dataPersistingObj in dataPersistingObjects)
        {
            dataPersistingObj.SaveData(ref gameData);
        }

        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersisting> FindAllDataPersistingObjects() 
    {
        IEnumerable<IDataPersisting> dataPersistingsObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersisting>()
            .OfType<IDataPersisting>();

        return new List<IDataPersisting>(dataPersistingsObjects);
    }
}
