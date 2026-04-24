using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private PlayerController _controller;
    [SerializeField] private CameraRotationController _cameraController;

    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private bool _isRunning;

    void Awake()
    {
        if (_controller == null)
            _controller = GetComponent<PlayerController>();
    }

    void Update()
    {
        // 입력값 전달
        _controller.SetMoveInput(_moveInput);
        _controller.SetRunning(_isRunning);

        _cameraController.SetLookInput(_lookInput);
    }

    public void OnMove(InputAction.CallbackContext context)
        => _moveInput = context.ReadValue<Vector2>();


    public void OnRun(InputAction.CallbackContext context)
        => _isRunning = context.ReadValueAsButton();


    public void OnLook(InputAction.CallbackContext context)
        => _lookInput = context.ReadValue<Vector2>();


    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
            _controller.RequestJump();
    }
}