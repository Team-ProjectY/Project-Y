using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

[CustomEditor(typeof(UIButton), true)]
[CanEditMultipleObjects]
public class UIButtonEditor : ButtonEditor
{
    private SerializedProperty _clickSound;
    private SerializedProperty _hoverSound;

    private SerializedProperty _buttonType;
    private SerializedProperty _enableObject;
    private SerializedProperty _disableObject;
    private SerializedProperty _nextSceneName;

    protected override void OnEnable()
    {
        base.OnEnable();

        _clickSound = serializedObject.FindProperty("clickSound");
        _hoverSound = serializedObject.FindProperty("hoverSound");

        _buttonType = serializedObject.FindProperty("buttonType");
        _enableObject = serializedObject.FindProperty("enableObject");
        _disableObject = serializedObject.FindProperty("disableObject");
        _nextSceneName = serializedObject.FindProperty("nextSceneName");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // ===== Sound =====
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Sound", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(_clickSound);
        EditorGUILayout.PropertyField(_hoverSound);

        // ===== Button Logic =====
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Button Logic", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(_buttonType);

        ButtonType type = (ButtonType)_buttonType.enumValueIndex;

        switch (type)
        {
            case ButtonType.ChangeCanvas:
                EditorGUILayout.PropertyField(_disableObject, new GUIContent("Object To Disable"));
                EditorGUILayout.PropertyField(_enableObject, new GUIContent("Object To Enable"));
                break;

            case ButtonType.OpenPopup:
                EditorGUILayout.PropertyField(_enableObject, new GUIContent("Popup To Open"));
                break;

            case ButtonType.ClosePopup:
                EditorGUILayout.PropertyField(_disableObject, new GUIContent("Popup To Close"));
                break;

            case ButtonType.GoScene:
                EditorGUILayout.PropertyField(_nextSceneName, new GUIContent("Scene Name"));
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
