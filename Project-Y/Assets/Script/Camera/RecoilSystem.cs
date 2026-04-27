using UnityEngine;

public class RecoilSystem : MonoBehaviour
{
    private float _recoilX;
    private float _recoilY;

    [SerializeField] private float _recoverSpeed = 8f;

    /// <summary>
    /// 반동 추가
    /// </summary>
    public void Add(float x, float y)
    {
        _recoilX += x;
        _recoilY += Random.Range(-y, y);
    }

    /// <summary>
    /// 현재 반동 반환 + 복구 처리
    /// </summary>
    public Vector2 Tick()
    {
        Vector2 recoil = new Vector2(_recoilX, _recoilY);

        _recoilX = Mathf.Lerp(_recoilX, 0f, Time.deltaTime * _recoverSpeed);
        _recoilY = Mathf.Lerp(_recoilY, 0f, Time.deltaTime * _recoverSpeed);

        return recoil;
    }
}