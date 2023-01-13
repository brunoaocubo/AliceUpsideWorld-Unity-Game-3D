using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{

    private string dataDirPath = " ";
    private string dataFileName = "";
    //private bool useEncryption = false;
    //private readonly string encryptionCodeWord = "word";

    public FileDataHandler(string dataDirPath, string dataFileName)//, bool useEncryption) 
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        //this.useEncryption = useEncryption;
    }

    public GameData Load() 
    {
        //Use Path.Combine para sincronizar com diferentes OS's com diferentes tipos de caminhos (Path).
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        GameData loadedData = null;
        if (File.Exists(fullPath)) 
        {
            try 
            {
                //Carregar os dados serializados do arquivo.
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open)) 
                {
                    using(StreamReader reader = new StreamReader(stream)) 
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                //Deserialize os dados do JSON de volta para o C# object.
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
                
                //if (useEncryption)
                //{
                //    dataToLoad = EncryptDecrypt(dataToLoad);
                //}
            }
            catch (Exception x)
            {
                Debug.LogError("Um erro aconteceu ao tentar carregar os dados do arquivo: " + fullPath + "\n" + x);
            }
        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        //Use Path.Combine para sincronizar com diferentes OS's com diferentes tipos de caminhos (Path).
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try 
        {
            //Crie o diretório no qual o arquivo será gravado, se ainda não existir.
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            //Serialize o objeto de dados do jogo C # em json.
            string dataToStore = JsonUtility.ToJson(data, true);

            //Gravar os dados serializados no arquivo.
            using(FileStream stream = new FileStream(fullPath, FileMode.Create)) 
            {
                using(StreamWriter writer = new StreamWriter(stream)) 
                {
                    writer.Write(dataToStore);
                }
            }

            //Optional encryption data.
            //if (useEncryption) 
            //{
              //  dataToStore = EncryptDecrypt(dataToStore);
            //}
        }
        catch (Exception x) 
        {
            Debug.LogError("Um erro aconteceu ao tentar salvar os dados do arquivo: " + fullPath + "\n" + x);
        }
    }

    //private string EncryptDecrypt(string data) 
    //{
    //    //string modifiedData = "";
    //    //for (int i = 0; i < data.Length; i++)
    //    //{
    //    //    modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
    //    //}
    //    //return modifiedData;
    //}
}


