using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponSOData _data;
    public WeaponSOData Data => _data;

    private WeaponController _controller;
    public bool IsInitialized { get; private set; } = false;

    private int _currentAmmo;

    /// <summary>
    /// 의존성을 주입하고 초기화 상태를 활성화한다.
    /// </summary>
    /// <param name="controller">무기들을 제어하는 컨트롤러</param>
    public void Init(WeaponController controller)
    {
        _controller = controller;
        // TODO: 초기화 상태

        IsInitialized = true;
    }

    void Awake()
    {
        _currentAmmo = _data.MagazineSize;
    }

    public void StartFire()
    {
        Fire();
    }

    public void StopFire()
    {
        // 단발 무기라서 별도 처리 없음
    }

    private void Fire()
    {
        // TODO: 발사 처리 (Raycast / Projectile)
        _currentAmmo--;

        // TODO: 반동 적용
        // TODO: 사운드 재생
        // TODO: 이펙트 생성

        // 반동 적용
        _controller.ApplyRecoil(_data.RecoilX, _data.RecoilY);
    }

    public void Reload()
    {
        // TODO: 재장전 로직
        _currentAmmo = _data.MagazineSize;
    }

    public void OnEquip()
    {
        gameObject.SetActive(true);
    }

    public void OnUnequip()
    {
        gameObject.SetActive(false);
        StopFire();
    }
}