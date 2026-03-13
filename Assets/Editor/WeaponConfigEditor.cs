using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WeaponConfig))]
public class WeaponConfigEditor : Editor
{
    private WeaponConfig _config;
    private SerializedProperty _attackConfigProp;

    private void OnEnable()
    {
        _config = (WeaponConfig)target;
        _attackConfigProp = serializedObject.FindProperty("_attackConfig");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        DrawPropertiesExcluding(serializedObject, "_attackConfig");
        
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("⚡ Стратегия стрельбы", EditorStyles.boldLabel);
        
        EditorGUI.BeginChangeCheck();
        
        EditorGUILayout.PropertyField(_attackConfigProp, new GUIContent("Shoot Config"));
        
        if (_attackConfigProp.objectReferenceValue != null && 
            !(_attackConfigProp.objectReferenceValue is IShootConfig))
        {
            EditorGUILayout.HelpBox(
                "Объект должен реализовывать IShootConfig!", 
                MessageType.Error
            );
        }
        
        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }
        
        if (_attackConfigProp.objectReferenceValue != null)
        {
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Настройки стратегии", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            
            var editor = Editor.CreateEditor(_attackConfigProp.objectReferenceValue);
            editor.OnInspectorGUI();
            
            EditorGUI.indentLevel--;
        }
        
        serializedObject.ApplyModifiedProperties();
    }
}