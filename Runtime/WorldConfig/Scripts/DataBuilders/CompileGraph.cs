using System;
using UnityEngine;

namespace HoangNam.WorldConfig
{
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
}
