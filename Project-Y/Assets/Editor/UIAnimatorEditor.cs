using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(UIAnimator))]
public class UIAnimatorEditor : Editor
{
    private SerializedProperty _uiAnimaType;

    /// <summary> FadeInOut필드 </summary>
    private SerializedProperty _canvasGroup;
    private SerializedProperty _fadeInDuration;
    private SerializedProperty _fadeOutDuration;

    /// <summary> ScaleInOut필드 </summary>
    private SerializedProperty _scaleInDuration;
    private SerializedProperty _scaleOutDuration;

    /// <summary> Move필드 </summary>
    private SerializedProperty _moveOffset;
    private SerializedProperty _moveDuration;

    private void OnEnable()
    {
        _uiAnimaType = serializedObject.FindProperty("_uiAnimaType");

        _canvasGroup = serializedObject.FindProperty("_canvasGroup");
        _fadeInDuration = serializedObject.FindProperty("_fadeInDuration");
        _fadeOutDuration = serializedObject.FindProperty("_fadeOutDuration");

        _scaleInDuration = serializedObject.FindProperty("_scaleInDuration");
        _scaleOutDuration = serializedObject.FindProperty("_scaleOutDuration");

        _moveOffset = serializedObject.FindProperty("_moveOffset");
        _moveDuration = serializedObject.FindProperty("_moveDuration");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Animation", EditorStyles.boldLabel);

        UIAnimaType type = (UIAnimaType)_uiAnimaType.intValue;
        EditorGUI.BeginChangeCheck();

        type = (UIAnimaType)EditorGUILayout.EnumFlagsField("Type", type);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Change UIPanel Type");
            _uiAnimaType.intValue = (int)type;
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
        }

        EditorGUILayout.Space(3);

        if (type.HasFlag(UIAnimaType.FadeInOut))
        {
            GUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField("Fade", EditorStyles.boldLabel);

            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_canvasGroup);
            EditorGUILayout.PropertyField(_fadeInDuration, new GUIContent("In Duration"));
            EditorGUILayout.PropertyField(_fadeOutDuration, new GUIContent("Out Duration"));
            EditorGUI.indentLevel--;

            GUILayout.EndVertical();
        }

        if (type.HasFlag(UIAnimaType.ScaleInOut))
        {
            GUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField("Scale", EditorStyles.boldLabel);

            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_scaleInDuration, new GUIContent("In Duration"));
            EditorGUILayout.PropertyField(_scaleOutDuration, new GUIContent("Out Duration"));
            EditorGUI.indentLevel--;

            GUILayout.EndVertical();
        }

        if (type.HasFlag(UIAnimaType.Moving))
        {
            GUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField("Move", EditorStyles.boldLabel);

            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_moveOffset, new GUIContent("Offset"));
            EditorGUILayout.PropertyField(_moveDuration, new GUIContent("Duration"));
            EditorGUI.indentLevel--;

            GUILayout.EndVertical();
        }

        serializedObject.ApplyModifiedProperties();
    }
}