using System;

namespace HoangNam.WorldConfig
{
  /// <summary>
  /// Marks a method to be invokable via a button in the Unity Inspector.
  /// Usage: [Button] or [Button("Custom Label")]
  /// </summary>
  [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
  public sealed class ButtonAttribute : Attribute
  {
    public string Label { get; }

    public ButtonAttribute() => Label = null;
    public ButtonAttribute(string label) => Label = label;
  }
}
