using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("속도")]
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float runSpeed = 7f;

    [SerializeField] private float jumpForce = 5.5f;

    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _camTransform;

    private Vector2 _moveInput;
    private bool _isRunning;

    private bool _isGrounded;
    // todo : 레이 길이 하드코딩 수정필요
    private float _rayDistance = 1.1f;
    [SerializeField] private LayerMask _groundMask;

    private bool _jumpRequested;

    void Awake()
    {
        // 인스펙터에서 할당되지 않은 경우 자동으로 컴포넌트 참조
        if (_rigidbody == null)
            _rigidbody = GetComponent<Rigidbody>();
        if (_camTransform == null)
            _camTransform = Camera.main.transform;
    }

    void Start()
    {
        _rigidbody.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        CheckGround();
        HandleMovement();
        if (_jumpRequested)
        {
            Jump();
            _jumpRequested = false;
        }
    }


    // PlayerInput에서 호출
    public void RequestJump() => _jumpRequested = true;
    public void SetMoveInput(Vector2 input) => _moveInput = input;
    public void SetRunning(bool isRunning) => _isRunning = isRunning;


    public void Jump()
    {
        if (!_isGrounded) return;
        _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void HandleMovement()
    {
        Vector3 forward = _camTransform.forward;
        Vector3 right = _camTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 dir = forward * _moveInput.y + right * _moveInput.x;

        float currentSpeed = _isRunning ? runSpeed : walkSpeed;

        Vector3 velocity = dir * currentSpeed;

        _rigidbody.linearVelocity = new Vector3(
            velocity.x,
            _rigidbody.linearVelocity.y,
            velocity.z
        );
    }

    private void CheckGround()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, _rayDistance, _groundMask);
    }
}