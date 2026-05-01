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
    private SerializedProperty _enablePanel;
    private SerializedProperty _disablePanel;
    private SerializedProperty _nextSceneName;

    protected override void OnEnable()
    {
        base.OnEnable();

        _clickSound = serializedObject.FindProperty("_clickSound");
        _hoverSound = serializedObject.FindProperty("_hoverSound");

        _buttonType = serializedObject.FindProperty("_buttonType");
        _enablePanel = serializedObject.FindProperty("_enablePanel");
        _disablePanel = serializedObject.FindProperty("_disablePanel");
        _nextSceneName = serializedObject.FindProperty("_nextSceneName");
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
                EditorGUILayout.PropertyField(_disablePanel, new GUIContent("Panel To Disable"));
                EditorGUILayout.PropertyField(_enablePanel, new GUIContent("Panel To Enable"));
                break;

            case ButtonType.OpenPopup:
                EditorGUILayout.PropertyField(_enablePanel, new GUIContent("Popup To Open"));
                break;

            case ButtonType.ClosePopup:
                EditorGUILayout.PropertyField(_disablePanel, new GUIContent("Popup To Close"));
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
