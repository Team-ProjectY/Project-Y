using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private PlayerMovement _playerMovement;//PlayerMovement에 있는 함수들을 쓰기위해 가져옴
    [SerializeField ]private CameraRotationController _cameraRotationController;//인스펙터에서 가져오기(추후에 수정가능성 있음)
    private Vector3 _movedir;//Vector3 입력값을 받고 함수에 전달하기 위한 변수
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
