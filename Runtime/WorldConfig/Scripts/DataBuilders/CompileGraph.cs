using System;
using UnityEngine;

public class CompiledGraph : ScriptableObject
{
  public NodeData[] nodes;
}

[Serializable]
public struct NodeData
{
  public string name;
  public float value;
}