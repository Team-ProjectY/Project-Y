using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private CameraRotationController _cameraController;

    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private bool _jumpRequested;

    void Awake()
    {
        // 인스펙터 미할당 시 자동 참조
        if (_playerMovement == null)
            _playerMovement = GetComponent<PlayerMovement>();
        if (_cameraController == null)
            _cameraController = GetComponentInChildren<CameraRotationController>();
    }

    void Update()
    {
        // 매 프레임 최신 입력값을 각 컴포넌트에 전달
        _playerMovement.SetMoveInput(_moveInput);
        _cameraController.SetLookInput(_lookInput);

        // 콜백에서 요청된 점프를 Update 타이밍에 처리
        // (점프 물리 처리는 PlayerMovement.FixedUpdate에서 수행)
        if (_jumpRequested)
        {
            _playerMovement.Jump();
            _jumpRequested = false;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        _lookInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
            _jumpRequested = true;
    }
}