using UnityEngine;

public class RaycastGroundChecker : MonoBehaviour, IGroundChecker
{
    [SerializeField] private float rayDistance = 1.1f;
    [SerializeField] private LayerMask groundMask;

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, rayDistance, groundMask);
    }
}