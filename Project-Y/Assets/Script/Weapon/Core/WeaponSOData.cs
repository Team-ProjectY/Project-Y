using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/WeaponData")]
public class WeaponSOData : ScriptableObject
{
    [Header("ADS")]
    public float adsFov = 40f;          // 기본 조준 FOV
    public float adsSpeed = 10f;        // 전환 속도
    public float sensitivityMultiplier = 0.5f; // 조준 시 감도 감소

    [Header("Scope")]
    public bool hasScope;
    public float scopeFov = 20f;        // 스코프 배율 (더 낮을수록 확대)
}