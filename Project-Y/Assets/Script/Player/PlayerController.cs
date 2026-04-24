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
    [SerializeField] private float _proneSpeed = 1f;
    [SerializeField] private float _jumpForce = 5.5f;

    // 상태
    private PostureState _posture = PostureState.Standing; private Vector2 _moveInput;
    private bool _isRunning;
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

        float speed = _walkSpeed;

        switch (_posture)
        {
            case PostureState.Prone:
                speed = _proneSpeed;
                break;

            case PostureState.Crouching:
                speed = _crouchSpeed;
                break;

            case PostureState.Standing:
                speed = _isRunning ? _runSpeed : _walkSpeed;
                break;
        }

        _movement.Move(dir, speed);
    }

    /// <summary>
    /// 점프 처리 (지면 체크 포함)
    /// 웅크리거나 엎드린 상태에서는 점프가 안됨
    /// </summary>
    void HandleJump()
    {
        if (_posture != PostureState.Standing)
            return;

        if (_jumpRequested && IsGrounded())
        {
            _movement.Jump(_jumpForce);
        }
    }

    /// <summary>
    /// 지면에 있는지 여부
    /// </summary>
    private bool IsGrounded()
    {
        return _groundChecker != null && _groundChecker.IsGrounded();
    }

    // Input에서 호출
    public void SetMoveInput(Vector2 input) => _moveInput = input;
    public void SetRunning(bool value) => _isRunning = value;
    public void RequestJump() => _jumpRequested = true;

    public void ToggleCrouch()
    {
        // 공중에서는 상태 변경 금지
        if (!IsGrounded())
            return;

        if (_posture == PostureState.Crouching)
            _posture = PostureState.Standing;
        else
            _posture = PostureState.Crouching;
    }

    public void ToggleProne()
    {
        // 공중에서는 상태 변경 금지
        if (!IsGrounded())
            return;

        if (_posture == PostureState.Prone)
            _posture = PostureState.Standing;
        else
            _posture = PostureState.Prone;
    }
}

