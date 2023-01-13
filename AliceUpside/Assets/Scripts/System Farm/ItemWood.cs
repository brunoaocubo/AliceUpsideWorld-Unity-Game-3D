using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class ItemWood : MonoBehaviour, IDataPersisting
{

    [SerializeField] private string id;
    private bool woodCollected;
    public void LoadData(GameData data)
    {
        data.woodCollected.TryGetValue(id, out woodCollected);
        if (woodCollected)
        {
            gameObject.SetActive(false);
        }
    }

    public void SaveData(ref GameData data)
    {
        if (data.woodCollected.ContainsKey(id))
        {
            data.woodCollected.Remove(id);
        }
        data.woodCollected.Add(id, woodCollected);
    }
}
*/