using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/WeaponSO")]
public class WeaponSOData : ScriptableObject
{
    /// <summary> 자동 사격 여부 (true: 연사, false: 단발) </summary>
    public bool IsAutomatic;
    /// <summary> 발사 속도 (초당 발사 횟수, RPS) </summary>
    public float FireRate;
    /// <summary> 탄창 용량 (최대 장전 가능 탄 수) </summary>
    public int MagazineSize;
    /// <summary> 재장전 시간 (초) </summary>
    public float ReloadTime;
    /// <summary> 사거리 (Raycast 최대 거리) </summary>
    public float Range;


    /// <summary> 조준 방식 </summary>
    public AimMode AimMode;
    /// <summary> ADS 시 카메라 시야각 (FOV) </summary>
    public float AimFov = 40f;
    /// <summary> ADS 전환 속도 (값이 클수록 빠르게 전환) </summary>
    public float AimSpeed = 10f;
    /// <summary> ADS 시 감도 배율 (마우스 감도 감소 비율) </summary>
    public float SensitivityMultiplier = 0.5f;


    /// <summary> 수직 반동 값 (위로 튀는 힘) </summary>
    public float RecoilX;
    /// <summary> 수평 반동 값 (좌우 흔들림) </summary>
    public float RecoilY;
}