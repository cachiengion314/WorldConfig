using UnityEngine;
using UnityEditor;
using Unity.Mathematics;

public class Range2Attribute : PropertyAttribute
{
  public float Min;
  public float Max;
  public Range2Attribute(float min, float max)
  {
    Min = min;
    Max = max;
  }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(Range2Attribute))]
public class Range2Drawer : PropertyDrawer
{
  public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
  {
    var attr = (Range2Attribute)attribute;
    var xProp = property.FindPropertyRelative("x");
    var yProp = property.FindPropertyRelative("y");

    var halfWidth = position.width / 2f - 2f;
    var rectX = new Rect(position.x, position.y, halfWidth, position.height);
    var rectY = new Rect(position.x + halfWidth + 4f, position.y, halfWidth, position.height);

    xProp.floatValue = EditorGUI.Slider(rectX, xProp.floatValue, attr.Min, attr.Max);
    yProp.floatValue = EditorGUI.Slider(rectY, yProp.floatValue, attr.Min, attr.Max);
    // enforce left <= right
    xProp.floatValue = math.min(xProp.floatValue, yProp.floatValue);
    yProp.floatValue = math.max(xProp.floatValue, yProp.floatValue);
  }
}
#endif