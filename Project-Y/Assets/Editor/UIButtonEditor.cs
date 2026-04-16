using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

[CustomEditor(typeof(UIButton), true)]
[CanEditMultipleObjects]
public class UIButtonEditor : ButtonEditor
{
    SerializedProperty clickSound;
    SerializedProperty hoverSound;

    protected override void OnEnable()
    {
        base.OnEnable();

        clickSound = serializedObject.FindProperty("clickSound");
        hoverSound = serializedObject.FindProperty("hoverSound");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // ===== Sound =====
        EditorGUILayout.LabelField("Sound", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(clickSound);
        EditorGUILayout.PropertyField(hoverSound);

        EditorGUILayout.Space();

        // ===== 기본 Button =====
        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Base Button", EditorStyles.boldLabel);

        base.OnInspectorGUI();

        serializedObject.ApplyModifiedProperties();
    }
}