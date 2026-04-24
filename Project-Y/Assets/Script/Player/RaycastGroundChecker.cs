using UnityEngine;

public class RaycastGroundChecker : MonoBehaviour, IGroundChecker
{
    [SerializeField] private float _rayDistance = 1.1f;
    [SerializeField] private LayerMask _groundMask;

    /// <summary>
    /// 아래 방향으로 Raycast를 쏴서 지면 체크
    /// </summary>
    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, _rayDistance, _groundMask);
    }
}