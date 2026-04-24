using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyMovement : MonoBehaviour, IMovement
{
    [SerializeField] private Rigidbody _rigidbody;

    void Awake()
    {
        if (_rigidbody == null)
            _rigidbody = GetComponent<Rigidbody>();

        _rigidbody.freezeRotation = true;
    }

    public void Move(Vector3 direction, float speed)
    {
        Vector3 velocity = direction * speed;

        _rigidbody.linearVelocity = new Vector3(
            velocity.x,
            _rigidbody.linearVelocity.y,
            velocity.z
        );
    }

    public void Jump(float force)
    {
        _rigidbody.AddForce(Vector3.up * force, ForceMode.Impulse);
    }
}