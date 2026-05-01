using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WeaponSOData))]
public class WeaponSODataEditor : Editor
{
    private SerializedProperty _isAutomatic;
    private SerializedProperty _fireRate;
    private SerializedProperty _magazineSize;
    private SerializedProperty _reloadTime;
    private SerializedProperty _range;

    private SerializedProperty _aimMode;
    private SerializedProperty _aimFovDivisor;
    private SerializedProperty _aimSpeed;
    private SerializedProperty _sensitivityMultiplier;

    private SerializedProperty _recoilX;
    private SerializedProperty _recoilY;

    void OnEnable()
    {
        _isAutomatic = serializedObject.FindProperty("IsAutomatic");
        _fireRate = serializedObject.FindProperty("FireRate");
        _magazineSize = serializedObject.FindProperty("MagazineSize");
        _reloadTime = serializedObject.FindProperty("ReloadTime");
        _range = serializedObject.FindProperty("Range");

        _aimMode = serializedObject.FindProperty("AimMode");
        _aimFovDivisor = serializedObject.FindProperty("AimFovDivisor");
        _aimSpeed = serializedObject.FindProperty("AimSpeed");
        _sensitivityMultiplier = serializedObject.FindProperty("SensitivityMultiplier");

        _recoilX = serializedObject.FindProperty("RecoilX");
        _recoilY = serializedObject.FindProperty("RecoilY");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawHeader("사격", Color.white);
        EditorGUILayout.PropertyField(_isAutomatic, new GUIContent("자동 사격"));
        EditorGUILayout.Slider(_fireRate, 0.01f, 20f, "발사 속도");
        EditorGUILayout.IntSlider(_magazineSize, 1, 100, "탄창 크기");
        EditorGUILayout.Slider(_reloadTime, 0.1f, 10f, "재장전 시간");
        EditorGUILayout.Slider(_range, 1f, 500f, "사거리");

        Space();

        DrawHeader("조준", Color.white);
        EditorGUILayout.PropertyField(_aimMode, new GUIContent("조준 타입"));
        EditorGUILayout.Slider(_aimFovDivisor, 1f, 5f, "조준 FOV (나누기 값)");
        EditorGUILayout.Slider(_aimSpeed, 1f, 20f, "조준 속도");
        EditorGUILayout.Slider(_sensitivityMultiplier, 0.1f, 1f, "감도 배율");

        Space();

        DrawHeader("반동", Color.white);
        EditorGUILayout.Slider(_recoilX, 0f, 10f, "반동 X");
        EditorGUILayout.Slider(_recoilY, 0f, 10f, "반동 Y");

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawHeader(string title, Color color)
    {
        GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
        style.fontSize = 13;
        style.normal.textColor = color;

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField(title, style);
        EditorGUILayout.Space(3);
    }

    private void Space()
    {
        EditorGUILayout.Space(8);
    }
}
