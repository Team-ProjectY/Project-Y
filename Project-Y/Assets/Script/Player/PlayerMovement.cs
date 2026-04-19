using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 6f;
    [SerializeField] private float jumpForce = 5.5f;

    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _camTransform;

    private Vector2 _moveInput;

    private bool _isGrounded;
    // todo : 하드코딩 수정필요
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
        Move();
        if (_jumpRequested)
        {
            Jump();
            _jumpRequested = false;
        }
    }


    public void RequestJump() => _jumpRequested = true;

    public void Jump()
    {
        if (!_isGrounded) return;
        _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }


    public void SetMoveInput(Vector2 input) => _moveInput = input;

    private void Move()
    {
        // 카메라의 forward/right를 기준으로 이동 방향 계산
        Vector3 forward = _camTransform.forward;
        Vector3 right = _camTransform.right;

        // 수직 성분 제거 (경사면에서도 수평 이동 유지)
        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        // 입력값과 카메라 방향을 조합해 최종 이동 방향 산출
        Vector3 dir = forward * _moveInput.y + right * _moveInput.x;

        Vector3 velocity = dir * speed;
        // y축 속도는 유지 (중력 및 점프 영향 보존)
        _rigidbody.linearVelocity = new Vector3(velocity.x, _rigidbody.linearVelocity.y, velocity.z);
    }


    private void CheckGround()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, _rayDistance, _groundMask);
    }
}