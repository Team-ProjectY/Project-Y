using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform _cam;
    // 이동 로직을 담당하는 컴포넌트 (인터페이스로 추상화)
    [SerializeField] private MonoBehaviour _movementComponent;
    // 바닥 체크 로직 컴포넌트 (인터페이스로 추상화)
    [SerializeField] private MonoBehaviour _groundCheckerComponent;

    private IMovement _movement;
    private IGroundChecker _groundChecker;

    // 값
    [SerializeField] private float _walkSpeed = 4f;
    [SerializeField] private float _runSpeed = 7f;
    [SerializeField] private float _crouchSpeed = 2f;
    [SerializeField] private float _jumpForce = 5.5f;

    // 상태
    private Vector2 _moveInput;
    private bool _isRunning;
    private bool _isCrouching;
    private bool _jumpRequested;

    void Awake()
    {
        // 인터페이스 캐싱
        _movement = _movementComponent as IMovement;
        _groundChecker = _groundCheckerComponent as IGroundChecker;
    }

    void Update()
    {
        HandleMovement();
        HandleJump();

        // 점프는 1프레임 요청 방식
        _jumpRequested = false;
    }

    /// <summary>
    /// 카메라 기준 이동 처리
    /// </summary>
    void HandleMovement()
    {
        Vector3 forward = _cam.forward;
        Vector3 right = _cam.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 dir = forward * _moveInput.y + right * _moveInput.x;

        // 상태별 속도 결정
        float speed;
        if (_isCrouching)
            speed = _crouchSpeed;
        else
            speed = _isRunning ? _runSpeed : _walkSpeed;

        _movement.Move(dir, speed);
    }

    /// <summary>
    /// 점프 처리 (지면 체크 포함)
    /// </summary>
    void HandleJump()
    {
        if (_jumpRequested && _groundChecker.IsGrounded())
        {
            _movement.Jump(_jumpForce);
        }
    }

    // Input에서 호출
    public void SetMoveInput(Vector2 input) => _moveInput = input;
    public void SetRunning(bool value) => _isRunning = value;
    public void RequestJump() => _jumpRequested = true;
    public void ToggleCrouch() => _isCrouching = !_isCrouching;
}