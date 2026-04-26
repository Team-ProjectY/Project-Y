using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private Weapon[] _weapons;
    [SerializeField] private MonoBehaviour _adsController;
    private IADSController _ads;

    private int _currentIndex = -1;
    private Weapon _currentWeapon;

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

        _ads?.EndADS();
    }
}