using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WeaponSOData))]
public class WeaponSODataEditor : Editor
{
    SerializedProperty IsAutomatic;
    SerializedProperty FireRate;
    SerializedProperty MagazineSize;
    SerializedProperty ReloadTime;
    SerializedProperty Range;

    SerializedProperty AimMode;
    SerializedProperty AimFov;
    SerializedProperty AimSpeed;
    SerializedProperty SensitivityMultiplier;

    SerializedProperty RecoilX;
    SerializedProperty RecoilY;

    void OnEnable()
    {
        IsAutomatic = serializedObject.FindProperty("IsAutomatic");
        FireRate = serializedObject.FindProperty("FireRate");
        MagazineSize = serializedObject.FindProperty("MagazineSize");
        ReloadTime = serializedObject.FindProperty("ReloadTime");
        Range = serializedObject.FindProperty("Range");

        AimMode = serializedObject.FindProperty("AimMode");
        AimFov = serializedObject.FindProperty("AimFov");
        AimSpeed = serializedObject.FindProperty("AimSpeed");
        SensitivityMultiplier = serializedObject.FindProperty("SensitivityMultiplier");

        RecoilX = serializedObject.FindProperty("RecoilX");
        RecoilY = serializedObject.FindProperty("RecoilY");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawHeader("사격", Color.white);
        EditorGUILayout.PropertyField(IsAutomatic, new GUIContent("자동 사격"));
        EditorGUILayout.Slider(FireRate, 0.01f, 20f, "발사 속도");
        EditorGUILayout.IntSlider(MagazineSize, 1, 100, "탄창 크기");
        EditorGUILayout.Slider(ReloadTime, 0.1f, 10f, "재장전 시간");
        EditorGUILayout.Slider(Range, 1f, 500f, "사거리");

        Space();

        DrawHeader("조준", Color.white);
        EditorGUILayout.PropertyField(AimMode, new GUIContent("조준 타입"));
        EditorGUILayout.Slider(AimFov, 20f, 90f, "조준 FOV");
        EditorGUILayout.Slider(AimSpeed, 1f, 20f, "조준 속도");
        EditorGUILayout.Slider(SensitivityMultiplier, 0.1f, 1f, "감도 배율");

        Space();

        DrawHeader("반동", Color.white);
        EditorGUILayout.Slider(RecoilX, 0f, 10f, "반동 X");
        EditorGUILayout.Slider(RecoilY, 0f, 10f, "반동 Y");

        serializedObject.ApplyModifiedProperties();
    }

    void DrawHeader(string title, Color color)
    {
        GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
        style.fontSize = 13;
        style.normal.textColor = color;

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField(title, style);
        EditorGUILayout.Space(3);
    }

    void Space()
    {
        EditorGUILayout.Space(8);
    }
}