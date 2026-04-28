using UnityEngine;

/// <summary>
/// CharacterController 기반 플레이어 이동 구현.
/// 최소 기능(수평 이동, 점프, 중력, 충돌)을 제공합니다.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class CharacterControllerMovement : MonoBehaviour, IMovement
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private float _gravity = -19.62f;
    [SerializeField] private float _groundedVerticalVelocity = -2f;

    private Vector3 _horizontalVelocity;
    private float _verticalVelocity;

    private void Awake()
    {
        if (_characterController == null)
            _characterController = GetComponent<CharacterController>();
        // 씬에 CharacterController가 빠져 있어도 런타임에서 자동 보강
        if (_characterController == null)
            _characterController = gameObject.AddComponent<CharacterController>();

        // CharacterController 단일 이동 경로를 위해 Rigidbody 물리는 비활성화
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        // CharacterController와 중복 충돌 계산을 피하기 위해 기존 캡슐 콜라이더 비활성화
        CapsuleCollider capsule = GetComponent<CapsuleCollider>();
        if (capsule != null)
            capsule.enabled = false;
    }

    private void Update()
    {
        if (_characterController == null)
            return;

        if (_characterController.isGrounded && _verticalVelocity < 0f)
            _verticalVelocity = _groundedVerticalVelocity;

        // 중력은 직접 누적 후 Move에 합산
        _verticalVelocity += _gravity * Time.deltaTime;

        Vector3 velocity = _horizontalVelocity + Vector3.up * _verticalVelocity;
        _characterController.Move(velocity * Time.deltaTime);
    }

    public void Move(Vector3 direction, float speed)
    {
        _horizontalVelocity = direction * speed;
    }

    public void Jump(float force)
    {
        if (_characterController == null || !_characterController.isGrounded)
            return;

        // 현재 구현은 jumpForce를 즉시 상향 속도로 사용
        _verticalVelocity = force;
    }
}
