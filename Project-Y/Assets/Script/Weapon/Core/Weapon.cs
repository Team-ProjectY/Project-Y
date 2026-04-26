using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponSOData _data;

    private bool _isFiring;

    public WeaponSOData Data => _data;

    public void StartFire()
    {
        /// TODO: 발사 조건 검사, 발사 구현
        /// 조건: 장전중, 단발/연사 ,발사 속도
    }

    public void StopFire()
    {
        // 연사 총 멈추기
        _isFiring = false;
    }

    public void Reload()
    {
        // TODO: 재장전
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