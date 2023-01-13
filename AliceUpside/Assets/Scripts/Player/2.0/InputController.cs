using UnityEngine.InputSystem;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class InputController : MonoBehaviour
{
    public static InputController Instance;

    private PlayerInput playerInput;
    private Vector2 inputMove;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction aimAction;
    private InputAction shootAction;
    private InputAction interactAction;
    private InputAction sprintAction;
    private InputAction item1Action;
    private InputAction item2Action;
    private InputAction item3Action;

    public Vector2 InputMove { get => inputMove; }
    public InputAction JumpAction { get => jumpAction; }
    public InputAction AimAction { get => aimAction; }
    public InputAction ShootAction { get => shootAction; }
    public InputAction InteractAction { get => interactAction; }
    public InputAction SprintAction { get => sprintAction; }
    public InputAction Item1Action { get => item1Action; }
    public InputAction Item2Action { get => item2Action; }
    public InputAction Item3Action { get => item3Action; }
    

    private void Awake()
    {
        if(Instance == null) 
        {
            Instance = this;
        }

        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        aimAction = playerInput.actions["Aim"];
        shootAction = playerInput.actions["Shoot"];
        interactAction = playerInput.actions["Interact"];
        sprintAction = playerInput.actions["Sprint"];
        item1Action = playerInput.actions["Item1"];
        item2Action = playerInput.actions["Item2"];
        item3Action = playerInput.actions["Item3"];
    }

    private void Update()
    {
        MethodInputMove();
    }

    private void MethodInputMove() 
    {
        inputMove = moveAction.ReadValue<Vector2>();
    }
}
