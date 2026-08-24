using UnityEngine;
using UnityEditor;

public class EnableIfAttribute : PropertyAttribute
{
  public string ConditionField;
  public EnableIfAttribute(string conditionField)
  {
    ConditionField = conditionField;
  }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(EnableIfAttribute))]
public class EnableIfDrawer : PropertyDrawer
{
  public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
  {
    var attr = (EnableIfAttribute)attribute;
    var conditionProp = property.serializedObject.FindProperty(attr.ConditionField);

    var wasEnabled = GUI.enabled;
    GUI.enabled = conditionProp != null && conditionProp.boolValue;
    EditorGUI.PropertyField(position, property, label, true);
    GUI.enabled = wasEnabled;
  }
}
#endif