using System;
using Unity.GraphToolkit.Editor;

[Serializable]
// Remove the second parameter so it doesn't look for an icon named "My Node"
[Node("My Category/My Node")]
[UseWithGraph(typeof(MyCustomGraph))]
public class MyCustomNode : Node
{
  const string k_ValueOptionName = "Value";

  protected override void OnDefineOptions(IOptionDefinitionContext context)
  {
    context.AddOption<float>(k_ValueOptionName)
        .WithDisplayName("Test Node")
        .WithDefaultValue(0f);
  }

  protected override void OnDefinePorts(IPortDefinitionContext context)
  {
    var valueOption = GetNodeOptionByName(k_ValueOptionName);
    valueOption.TryGetValue<float>(out var value);

    context.AddInputPort<float>("Value").WithDefaultValue(value).Build();
    context.AddOutputPort<float>("Result").Build();
  }
}