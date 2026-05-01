using UnityEngine;

public class PlayerController : MonoBehaviour, IPostureProvider
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
    private PostureState _posture = PostureState.Standing;
    public PostureState Posture { get { return _posture; } private set { _posture = value; } }

    private Vector2 _moveInput;
    private bool _isRunning;
    private bool _jumpRequested;

    void Awake()
    {
        // CharacterController 이동을 우선 적용 (이슈 #11 최소 요구사항)
        if (_movementComponent == null)
        {
            CharacterControllerMovement ccMovement = GetComponent<CharacterControllerMovement>();
            if (ccMovement == null)
                ccMovement = gameObject.AddComponent<CharacterControllerMovement>();

            _movementComponent = ccMovement;
        }

        // 인터페이스 캐싱
        _movement = _movementComponent as IMovement;
        _groundChecker = _groundCheckerComponent as IGroundChecker;
    }

    void Update()
    {
        // CharacterController 기반 이동은 Update 루프에서 처리
        HandleMovement();
        HandleJump();

        _jumpRequested = false;
    }

    /// <summary>
    /// 카메라 기준 이동 처리
    /// </summary>
    private void HandleMovement()
    {
        Vector3 forward = _cam.forward;
        Vector3 right = _cam.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 dir = forward * _moveInput.y + right * _moveInput.x;

        float speed = _walkSpeed;

        switch (Posture)
        {
            case PostureState.Proning:
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
    private void HandleJump()
    {
        if (Posture != PostureState.Standing)
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

        if (Posture == PostureState.Crouching)
            Posture = PostureState.Standing;
        else
            Posture = PostureState.Crouching;
    }

    public void ToggleProne()
    {
        // 공중에서는 상태 변경 금지
        if (!IsGrounded())
            return;

        if (Posture == PostureState.Proning)
            Posture = PostureState.Standing;
        else
            Posture = PostureState.Proning;
    }
}

