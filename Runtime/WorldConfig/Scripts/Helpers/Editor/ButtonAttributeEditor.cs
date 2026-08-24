#if UNITY_EDITOR
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace HoangNam
{
  /// <summary>
  /// Custom editor that scans any MonoBehaviour for [Button]-decorated methods
  /// and renders them as inspector buttons.
  /// Drop this file anywhere under an Editor/ folder.
  /// </summary>
  [CustomEditor(typeof(MonoBehaviour), editorForChildClasses: true)]
  [CanEditMultipleObjects]
  public class ButtonAttributeEditor : Editor
  {
    private MethodInfo[] _buttonMethods;

    private void OnEnable()
    {
      _buttonMethods = target
        .GetType()
        .GetMethods(BindingFlags.Instance | BindingFlags.Static |
                    BindingFlags.Public | BindingFlags.NonPublic)
        .Where(m => m.GetCustomAttribute<ButtonAttribute>() != null
                  && m.GetParameters().Length == 0)
        .ToArray();
    }

    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();

      if (_buttonMethods.Length == 0) return;

      EditorGUILayout.Space(4);

      foreach (var method in _buttonMethods)
      {
        var attr = method.GetCustomAttribute<ButtonAttribute>();
        var label = string.IsNullOrEmpty(attr.Label)
            ? ObjectNames.NicifyVariableName(method.Name)
            : attr.Label;

        if (GUILayout.Button(label))
        {
          foreach (var t in targets)
          {
            Undo.RecordObject(t, label);
            method.Invoke(t, null);
            EditorUtility.SetDirty(t);
          }
        }
      }
    }
  }
#endif
}
