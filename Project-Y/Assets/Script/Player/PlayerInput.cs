using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    [SerializeField] private CameraRotationController cameraController;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool isRunning;

    void Awake()
    {
        if (controller == null)
            controller = GetComponent<PlayerController>();
    }

    void Update()
    {
        controller.SetMoveInput(moveInput);
        controller.SetRunning(isRunning);

        cameraController.SetLookInput(lookInput);
    }

    public void OnMove(InputAction.CallbackContext context)
        => moveInput = context.ReadValue<Vector2>();

    public void OnRun(InputAction.CallbackContext context)
        => isRunning = context.ReadValueAsButton();

    public void OnLook(InputAction.CallbackContext context)
        => lookInput = context.ReadValue<Vector2>();

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
            controller.RequestJump();
    }
}