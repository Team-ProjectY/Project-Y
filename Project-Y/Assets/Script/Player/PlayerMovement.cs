using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private float _jumpForce = 10f;
    void Awake()
    {
        TryGetComponent<Rigidbody>(out _rigidbody);
    }

    public void Jump()
    {
        _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }

    public void Move(Vector3 movedir)
    {
        _rigidbody.linearVelocity = 
            new Vector3(movedir.x, _rigidbody.linearVelocity.y, movedir.z);
    }
}
