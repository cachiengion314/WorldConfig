using UnityEngine;
using UnityEditor;

namespace HoangNam.WorldConfig
{
  public class DisableIfAttribute : PropertyAttribute
  {
    public string ConditionField;
    public DisableIfAttribute(string conditionField)
    {
      ConditionField = conditionField;
    }
  }

#if UNITY_EDITOR
  [CustomPropertyDrawer(typeof(DisableIfAttribute))]
  public class DisableIfDrawer : PropertyDrawer
  {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      var attr = (DisableIfAttribute)attribute;
      var conditionProp = property.serializedObject.FindProperty(attr.ConditionField);

      var wasEnabled = GUI.enabled;
      GUI.enabled = conditionProp != null && !conditionProp.boolValue; // negated
      EditorGUI.PropertyField(position, property, label, true);
      GUI.enabled = wasEnabled;
    }
  }
#endif
}
