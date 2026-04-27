using UnityEngine;

public class CameraRotationController : MonoBehaviour
{
    [SerializeField] private Transform _playerBody;
    [SerializeField] private LookRotation _look;
    [SerializeField] private RecoilSystem _recoil;

    private float _xRot; // Pitch (상하)
    private float _yRot; // Yaw (좌우)

    void Update()
    {
        // 1. 입력 회전값
        Vector2 input = _look.GetLookDelta();

        // 2. 반동 값
        Vector2 recoilOffset = _recoil.Tick();

        // 3. 입력 누적 (반동은 최종 적용 시점에만 가산)
        _yRot += input.x;
        _xRot -= input.y;

        // 4. 상하 제한
        _xRot = Mathf.Clamp(_xRot, -85f, 85f);

        // 5. 카메라 적용 (Pitch + Recoil)
        transform.localRotation = Quaternion.Euler(_xRot - recoilOffset.x, 0f, 0f);

        // 6. 플레이어 회전 (Yaw + Recoil)
        _playerBody.rotation = Quaternion.Euler(0f, _yRot + recoilOffset.y, 0f);
    }

    /// <summary>
    /// 외부에서 반동 추가
    /// </summary>
    public void AddRecoil(float x, float y)
    {
        _recoil.Add(x, y);
    }
}
