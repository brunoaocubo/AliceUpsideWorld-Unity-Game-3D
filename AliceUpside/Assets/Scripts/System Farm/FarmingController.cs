using UnityEngine;
using UnityEngine.InputSystem;


public class FarmingController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    void Update()
    {
        Farming();
        Harverst();
    }

    private void Farming() 
    {
        if (InputController.Instance.InteractAction.triggered)
        {
            var slotPlant = RaycastDetect.Instance.InteractInfo.transform.GetComponent<SlotPlant>();
            if (slotPlant != null)
            {
                if(playerController.QuantitySeed > 0) 
                {
                    slotPlant.IncreasePlantInSlot();
                }
            }
        }
    }
    
    private void Harverst() 
    {
        if (InputController.Instance.InteractAction.triggered) 
        {
            var objPlant = RaycastDetect.Instance.InteractInfo.transform.GetComponentInChildren<CyclePlant>();

            if (objPlant != null) 
            {
                if (objPlant.HarverstIsReady) 
                {
                    playerController.IncreaseAmountSeeds();
                    Destroy(objPlant.gameObject);
                }
            }
        }
    }
}
