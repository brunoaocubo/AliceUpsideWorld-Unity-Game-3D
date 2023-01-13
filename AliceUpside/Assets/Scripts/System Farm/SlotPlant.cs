using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotPlant : MonoBehaviour
{
    [SerializeField] private GameObject plantPrefab;

    public void IncreasePlantInSlot() 
    {  
        if (transform.childCount <= 0)
        {
            PlayerController.Instance.DecreaseAmountSeeds();

            GameObject plant = Instantiate(plantPrefab, transform.position, Quaternion.identity);
            plant.transform.localScale = Vector3.one;
            plant.transform.SetParent(gameObject.transform);
        }
    }
}
