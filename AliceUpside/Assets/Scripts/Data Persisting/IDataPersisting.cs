using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersisting 
{
    void LoadData(GameData data);
    void SaveData(ref GameData data);
}
