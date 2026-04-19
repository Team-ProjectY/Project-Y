using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    [SerializeField ]private CameraRotationController _cameraRotationController;//인스펙터에서 가져오기(추후에 수정가능성 있음)
    private Vector3 _movedir;
    void Awake()
    {
        TryGetComponent<PlayerMovement>(out _playerMovement);
    }
    void FixedUpdate()
    {
        _playerMovement.Move(_movedir);
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        _movedir = context.ReadValue<Vector3>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
            _playerMovement.Jump();
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        _cameraRotationController._mousepos = context.ReadValue<Vector2>();
    }
}
