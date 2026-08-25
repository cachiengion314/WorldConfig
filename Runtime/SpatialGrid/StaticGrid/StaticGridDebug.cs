using UnityEngine;
using Unity.Mathematics;

#if UNITY_EDITOR
using UnityEditor;
#endif

public partial class StaticGridDebug : MonoBehaviour
{
#if UNITY_EDITOR
  public StaticGrid[] AuthoringGrids;
  /// <summary>
  /// DEBUG
  /// </summary>
  [Header("Debug Visualization")]
  [SerializeField] Color DebugCellColor = Color.green;
  [Range(0, 1)]
  [SerializeField] float DebugCellScale = 1;

  // New fields to control the text appearance
  [SerializeField] bool Show = true;
  [SerializeField] bool ShowOnyOneWiredBox;
  [SerializeField] bool ShowDebugText;
  [SerializeField] Color DebugTextColor = Color.lightGreen;
  [Range(10, 50)]
  [SerializeField] int DebugFontSize = 14;

  private void OnDrawGizmos()
  {
    if (!Show || AuthoringGrids == null) return;

    Gizmos.color = DebugCellColor;

    // Create a custom style for the text
    var labelStyle = new GUIStyle();
    labelStyle.normal.textColor = DebugTextColor;
    labelStyle.fontSize = math.max(0, DebugFontSize);
    labelStyle.alignment = TextAnchor.MiddleCenter; // Centers the text over the position

    foreach (var grid in AuthoringGrids)
    {
      if (ShowOnyOneWiredBox)
      {
        var gridSize = grid.CellSize * grid.Resolution;
        var worldPos = grid.Origin + .5f * gridSize;
        Gizmos.DrawWireCube(worldPos, gridSize);
        continue;
      }

      for (int x = 0; x < grid.Resolution.x; x++)
      {
        for (int y = 0; y < grid.Resolution.y; y++)
        {
          for (int z = 0; z < grid.Resolution.z; z++)
          {
            var gridPos = new int3(x, y, z);
            var worldPos = grid.ConvertGridToWorld(gridPos);
            Gizmos.DrawWireCube(worldPos, grid.CellSize * DebugCellScale);

            var index = grid.ConvertGridToIndex(gridPos);

            // Pass the custom style here
            if (ShowDebugText)
              Handles.Label(worldPos, $"{index}", labelStyle);
          }
        }
      }
    }
  }
#endif
}