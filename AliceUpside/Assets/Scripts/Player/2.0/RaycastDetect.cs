using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastDetect : MonoBehaviour
{
    public static RaycastDetect Instance;

    [SerializeField] private float hitRangeRay;
    [SerializeField] private float interactRangeRay;
    [SerializeField] private LayerMask layerMask;

    private RaycastHit hitInfo;
    private RaycastHit interactInfo;
    private Camera mainCamera;

    //Encapsulados
    public RaycastHit HitInfo { get => hitInfo; set => hitInfo = value; }
    public RaycastHit InteractInfo { get => interactInfo; set => interactInfo = value; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        mainCamera = Camera.main;
    }

    private void Update()
    {
        CheckRaycast();
    }

    private void CheckRaycast()
    {
        //Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hitInfo, hitRangeRay);
        Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out interactInfo, interactRangeRay);
    }
}
