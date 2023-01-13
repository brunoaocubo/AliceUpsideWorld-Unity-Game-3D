using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectItems : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    private void Update()
    {
        CollectWood();
    }

    private void CollectWood() 
    {
        if (InputController.Instance.InteractAction.triggered)
        {
            var generator = RaycastDetect.Instance.InteractInfo.transform.gameObject;
            if (generator.CompareTag("Generator")) 
            {
                playerController.DecreaseAmountWood();
            }        
        }
        if (InputController.Instance.InteractAction.triggered) 
        {
            var woodHit = RaycastDetect.Instance.InteractInfo.transform.gameObject;
            if (woodHit.CompareTag("Item_Wood"))
            {
                playerController.IncreaseAmountWood();
                Destroy(woodHit);
            }
        }
    }
}
