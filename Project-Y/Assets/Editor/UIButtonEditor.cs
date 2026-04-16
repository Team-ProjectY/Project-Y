using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

[CustomEditor(typeof(UIButton), true)]
[CanEditMultipleObjects]
public class UIButtonEditor : ButtonEditor
{
    SerializedProperty clickSound;
    SerializedProperty hoverSound;

    SerializedProperty buttonType;
    SerializedProperty enableObject;
    SerializedProperty disableObject;
    SerializedProperty nextSceneName;

    protected override void OnEnable()
    {
        base.OnEnable();

        clickSound = serializedObject.FindProperty("clickSound");
        hoverSound = serializedObject.FindProperty("hoverSound");

        buttonType = serializedObject.FindProperty("buttonType");
        enableObject = serializedObject.FindProperty("enableObject");
        disableObject = serializedObject.FindProperty("disableObject");
        nextSceneName = serializedObject.FindProperty("nextSceneName");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // ===== Sound =====
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Sound", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(clickSound);
        EditorGUILayout.PropertyField(hoverSound);

        // ===== Button Logic =====
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Button Logic", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(buttonType);

        ButtonType type = (ButtonType)buttonType.enumValueIndex;

        switch (type)
        {
            case ButtonType.ChangeCanvas:

            case ButtonType.OpenPopup:
                EditorGUILayout.PropertyField(enableObject, new GUIContent("Popup To Close"));
                EditorGUILayout.PropertyField(enableObject, new GUIContent("Popup To Open"));
                break;

            case ButtonType.ClosePopup:
                EditorGUILayout.PropertyField(disableObject, new GUIContent("Popup To Close"));
                break;

            case ButtonType.GoScene:
                EditorGUILayout.PropertyField(nextSceneName, new GUIContent("Scene Name"));
                break;

            case ButtonType.Quit:
                EditorGUILayout.HelpBox("게임 종료 버튼", MessageType.None);
                break;
        }

        serializedObject.ApplyModifiedProperties();

        // ===== 기본 Button =====
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Base Button", EditorStyles.boldLabel);
        base.OnInspectorGUI();
    }
}