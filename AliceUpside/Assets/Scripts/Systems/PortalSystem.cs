using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSystem : MonoBehaviour
{
    CharacterController characterController;
    [SerializeField] private Transform spawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        characterController = other.gameObject.GetComponent<CharacterController>();
        characterController.enabled = false;
        other.gameObject.transform.position = spawnPoint.transform.position;
        characterController.enabled = true;

    }
}
