using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private float _jumpForce = 10f;
    private float _normalspeed = 10f;
    private bool _isGrounded;
    private float _rayDistance;
    private CapsuleCollider _collider;
    //이 콜라이더는 임시용입니다 플레이어에 들어갈 콜라이더로 변경해야됩니다.
    void Awake()
    {
        TryGetComponent<Rigidbody>(out _rigidbody); 
        TryGetComponent<CapsuleCollider>(out _collider);
        _rigidbody.freezeRotation = true;
        _rayDistance = _collider != null ? (_collider.height / 2f) + 0.1f : 1.1f;
    }

    void FixedUpdate()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, _rayDistance, LayerMask.GetMask("Ground"));
    }

    public void Jump()
    {
        if(_isGrounded)
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }

    public void Move(Vector3 movedir)
    {
        // 카메라 기준으로 이동 방향 변환
        Transform cam = Camera.main.transform;

        Vector3 forward = cam.forward;
        Vector3 right = cam.right;

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
