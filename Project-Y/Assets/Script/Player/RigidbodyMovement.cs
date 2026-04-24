using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyMovement : MonoBehaviour, IMovement
{
    [SerializeField] private Rigidbody _rigidbody;

    void Awake()
    {
        if (_rigidbody == null)
            _rigidbody = GetComponent<Rigidbody>();

        // 회전 고정 (FPS 기준)
        _rigidbody.freezeRotation = true;
    }

    /// <summary>
    /// 수평 이동 (y속도 유지)
    /// </summary>
    public void Move(Vector3 direction, float speed)
    {
        Vector3 velocity = direction * speed;

        _rigidbody.linearVelocity = new Vector3(
            velocity.x,
            _rigidbody.linearVelocity.y,
            velocity.z
        );
    }

    /// <summary>
    /// 점프 (Impulse)
    /// </summary>
    public void Jump(float force)
    {
        _rigidbody.AddForce(Vector3.up * force, ForceMode.Impulse);
    }
}