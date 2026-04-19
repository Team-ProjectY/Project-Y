using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _jumpForce = 5.7f;
    [SerializeField] private float _normalspeed = 10f;
    [SerializeField] private bool _isGrounded;
    [SerializeField] private float _rayDistance;

    //레이어 마스크 캐싱하기 위한 변수
    private LayerMask _layerMask;
    
    //Move함수에 있는 Transform값을 캐싱하기 위한 변수
    private Transform _transform;
    
    private Rigidbody _rigidbody;
    
    //이 콜라이더는 임시용입니다 플레이어에 들어갈 콜라이더로 변경해야됩니다.
    [SerializeField] private CapsuleCollider _collider;
    

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.freezeRotation = true;

        if (TryGetComponent<CapsuleCollider>(out _collider))
            _rayDistance = (_collider.height / 2f) + 0.1f;
        else
            _rayDistance = 1.1f;
        _layerMask = LayerMask.GetMask("Ground");
    }

    void FixedUpdate()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, _rayDistance, _layerMask);
    }

    public void Jump()
    {
        if(_isGrounded)
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }

    public void Move(Vector3 movedir)
    {
        // 카메라 기준으로 이동 방향 변환
        _transform = Camera.main.transform;

        Vector3 forward = _transform.forward;
        Vector3 right = _transform.right;

        // Y축 제거 (경사면에서 위로 날아가지 않도록)
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 worldDir = (forward * movedir.z + right * movedir.x).normalized;

        _rigidbody.linearVelocity =
            new Vector3(worldDir.x * _normalspeed, _rigidbody.linearVelocity.y, worldDir.z * _normalspeed);
    }
}
