using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Unity.GraphToolkit.Editor;
using System.Collections.Generic;

[Graph("mygraph")]
[Serializable]
public class MyCustomGraph : Graph
{
  public void Compile(CompiledGraph output)
  {
    var list = new List<NodeData>();

    foreach (var node in GetNodes())
    {
      if (node is MyCustomNode myNode)
      {
        myNode.GetNodeOptionByName("Value").TryGetValue<float>(out var value);
        list.Add(new NodeData { name = myNode.Title, value = value });
      }
    }

    output.nodes = list.ToArray();
    EditorUtility.SetDirty(output);
    AssetDatabase.SaveAssets();
  }

  [MenuItem("Assets/Graph Toolkit Action/Compile Selected Graph", false, 20)]
  public static void CompileSelectedGraph()
  {
    UnityEngine.Object selectedAsset = Selection.activeObject;
    if (selectedAsset == null) return;

    string assetPath = AssetDatabase.GetAssetPath(selectedAsset);

    if (!assetPath.EndsWith(".mygraph", StringComparison.OrdinalIgnoreCase))
    {
      Debug.LogWarning("Selected asset is not a valid MyCustomGraph asset.");
      return;
    }

    MyCustomGraph graph = GraphDatabase.LoadGraph<MyCustomGraph>(assetPath);
    if (graph == null)
    {
      Debug.LogError("Failed to load the graph asset data.");
      return;
    }

    // --- AUTOMATIC OUTPUT DETECTION/CREATION ---
    // Get the folder and name of your current graph file
    string directory = Path.GetDirectoryName(assetPath);
    string filename = Path.GetFileNameWithoutExtension(assetPath);
    string outputPath = Path.Combine(directory, filename + "_Compiled.asset");

    // Try to load an existing CompiledGraph asset at that location
    CompiledGraph targetOutput = AssetDatabase.LoadAssetAtPath<CompiledGraph>(outputPath);

    // If it doesn't exist yet, create one automatically
    if (targetOutput == null)
    {
      targetOutput = ScriptableObject.CreateInstance<CompiledGraph>();
      AssetDatabase.CreateAsset(targetOutput, outputPath);
      AssetDatabase.SaveAssets();
      Debug.Log($"Created new CompiledGraph asset at: {outputPath}");
    }
    // --------------------------------------------

    // Run your compile process using the detected/created asset
    graph.Compile(targetOutput);

    // Highlight the compiled asset in the project view so you can see it
    ProjectWindowUtil.ShowCreatedAsset(targetOutput);
    Debug.Log($"{graph.Name} compiled successfully to {outputPath}!");
  }

  [MenuItem("Assets/Create/My Custom Graph")]
  static void CreateAsset()
  {
    GraphDatabase.PromptInProjectBrowserToCreateNewAsset<MyCustomGraph>();
  }
}