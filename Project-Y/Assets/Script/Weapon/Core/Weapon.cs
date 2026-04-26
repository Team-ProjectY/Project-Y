using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponSOData _data;

    private float _lastFireTime;
    private int _currentAmmo;
    private bool _isReloading;
    private bool _isFiring;

    public WeaponSOData Data => _data;

    void Awake()
    {
        _currentAmmo = Data.MagazineSize;
    }

    void Update()
    {
        if (_isFiring)
            TryFire();
    }

    public void StartFire()
    {
        if (_isReloading) return;

        if (Data.IsAutomatic)
            _isFiring = true;
        else
            TryFire();
    }

    public void StopFire()
    {
        _isFiring = false;
    }

    void TryFire()
    {
        if (Time.time < _lastFireTime + (1f / Data.FireRate))
            return;

        if (_currentAmmo <= 0)
        {
            Reload();
            return;
        }

        Fire();
    }

    void Fire()
    {
        _lastFireTime = Time.time;
        _currentAmmo--;

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        if (Physics.Raycast(ray, out RaycastHit hit, Data.Range))
        {
            // TODO: 데미지 처리
        }

        // TODO: 반동
    }

    public void Reload()
    {
        if (_isReloading) return;

        _isReloading = true;
        Invoke(nameof(FinishReload), Data.ReloadTime);
    }

    void FinishReload()
    {
        _currentAmmo = Data.MagazineSize;
        _isReloading = false;
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