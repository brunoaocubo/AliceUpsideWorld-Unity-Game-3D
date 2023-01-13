using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class SwitchCamPlayer : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Canvas aimCanvas;
    [SerializeField] private Canvas hipefireCanvas;

    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private CinemachineFollowZoom aimCam;

    void Awake()
    {
        aimCanvas.enabled = false;
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        if (InputController.Instance.AimAction.IsPressed())
        {
            StartAim();
        }
        else 
        {
            CancelAim();
        }
    }

    private void OnEnable()
    {
        //InputController.Instance.AimAction.performed += _ => StartAim();
        //InputController.Instance.AimAction.canceled += _ => CancelAim();
    }

    private void OnDisable()
    {
        //InputController.Instance.AimAction.performed -= _ => StartAim();
        //InputController.Instance.AimAction.canceled -= _ => CancelAim();
    }

    private void StartAim() 
    {
        aimCam.m_Width = 1;
        aimCanvas.enabled = true;
        hipefireCanvas.enabled = false;
    }

    private void CancelAim()
    {
        aimCam.m_Width = 2;
        aimCanvas.enabled = false;
        hipefireCanvas.enabled = true;
    }
}
