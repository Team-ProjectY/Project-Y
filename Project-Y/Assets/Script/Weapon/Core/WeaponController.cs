using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private CameraRotationController _cameraRotationController;

    [SerializeField] private Weapon[] _weapons;
    [SerializeField] private MonoBehaviour _adsController;
    private IADSController _ads;

    private int _currentIndex = -1;
    [SerializeField] private Weapon _currentWeapon;

    void Awake()
    {
        _ads = _adsController as IADSController;
    }

    void Start()
    {
        Equip(0);
    }

    public void StartFire()
    {
        _currentWeapon?.StartFire();
    }

    public void StopFire()
    {
        _currentWeapon?.StopFire();
    }

    public void Reload()
    {
        _currentWeapon?.Reload();
    }

    public void StartAim()
    {
        if (_currentWeapon == null) return;
        _ads?.StartADS(_currentWeapon.Data);
    }

    public void StopAim()
    {
        _ads?.EndADS();
    }

    public void Equip(int index)
    {
        if (index < 0 || index >= _weapons.Length) return;
        if (_currentIndex == index) return;

        if (_currentWeapon != null)
            _currentWeapon.OnUnequip();

        _currentIndex = index;
        _currentWeapon = _weapons[index];

        _currentWeapon.OnEquip();

        // 의존성 주입
        if (!_currentWeapon.IsInitialized)
            _currentWeapon.Init(this);

        _ads?.EndADS();
    }

    /// <summary>
    /// Weapon에서 호출되는 반동 전달 함수
    /// </summary>
    public void ApplyRecoil(float x, float y)
    {
        _cameraRotationController.AddRecoil(x, y);
    }
}