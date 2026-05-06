using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponSOData _data;
    public WeaponSOData Data => _data;

    private WeaponController _controller;

    /// <summary>컨트롤러 의존성 주입이 끝나 사용 가능한 상태인지 나타냅니다.</summary>
    public bool IsInitialized { get; private set; } = false;


    private int _currentAmmo;
    private bool _isTriggerHeld;
    private bool _hasFiredThisTriggerPull;
    private float _nextFireTime;

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

    void Update()
    {
        // 입력중이 아니거나, 자동 총이 아니라면 
        if (!_isTriggerHeld || !_data.IsAutomatic)
            return;

        TryFire();
    }

    /// <summary>
    /// 발사 입력 시작을 처리하고, 즉시 1회 발사를 시도합니다.
    /// </summary>
    public void StartFire()
    {
        _isTriggerHeld = true;
        TryFire();
    }

    /// <summary>
    /// 발사 입력 종료를 처리하고 단발 사격 입력 상태를 초기화합니다.
    /// </summary>
    public void StopFire()
    {
        _isTriggerHeld = false;
        _hasFiredThisTriggerPull = false;
    }

    private void TryFire()
    {
        if (!CanFire())
            return;

        Fire();
    }

    private bool CanFire()
    {
        if (!IsInitialized || _data == null)
            return false;

        if (_currentAmmo <= 0)
            return false;

        if (Time.time < _nextFireTime)
            return false;

        if (!_data.IsAutomatic && _hasFiredThisTriggerPull)
            return false;

        return true;
    }

    private void Fire()
    {
        // TODO: 발사 처리 (Raycast / Projectile)
        _currentAmmo--;
        _hasFiredThisTriggerPull = true;

        float fireInterval = Mathf.Max(0.01f, _data.FireRate);
        _nextFireTime = Time.time + fireInterval;

        // TODO: 사운드 재생
        // TODO: 이펙트 생성

        // 반동 적용
        _controller.ApplyRecoil(_data.RecoilX, _data.RecoilY);
    }

    public void Reload()
    {
        // TODO: 재장전 로직
        _currentAmmo = _data.MagazineSize;
        _hasFiredThisTriggerPull = false;
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
