using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/WeaponSO")]
public class WeaponSOData : ScriptableObject
{
    public bool IsAutomatic;
    public float FireRate;
    public int MagazineSize;
    public float ReloadTime;
    public float Range;

    public float AdsFov = 40f;
    public float AdsSpeed = 10f;
    public float SensitivityMultiplier = 0.5f;

    public bool HasScope;
    public float ScopeFov = 20f;

    public float RecoilX;
    public float RecoilY;
}