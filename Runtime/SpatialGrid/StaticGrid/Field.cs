using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;

namespace HoangNam.WorldConfig
{
  public partial struct StaticGrid : IComponentData
  {
    public readonly float3 ConvertIndexToNormalize(in int index)
    {
      return ConvertIndexToNormalize(index, Resolution);
    }

    public readonly float3 ConvertWorldToNormalize(in float3 pos)
    {
      return ConvertWorldToNormalize(pos, Origin, Resolution, CellSize);
    }

    public readonly float3 ConvertGridToNormalize(in int3 gridPos)
    {
      return ConvertGridToNormalize(gridPos, Resolution);
    }

    public static float3 ConvertIndexToNormalize(in int index, in int3 resolution)
    {
      var gridPos = ConvertIndexToGrid(index, resolution);
      return ConvertGridToNormalize(gridPos, resolution);
    }

    public static float3 ConvertWorldToNormalize(
      in float3 pos,
      in float3 origin,
      in int3 resolution,
      in float3 cellSize
    )
    {
      var gridPos = ConvertWorldToGrid(pos, origin, cellSize);
      return ConvertGridToNormalize(gridPos, resolution);
    }

    public static float3 ConvertGridToNormalize(in int3 gridPos, in int3 resolution)
    {
      var uv = new float3(
        gridPos.x * 1.0f / resolution.x,
        gridPos.y * 1.0f / resolution.y,
        gridPos.z * 1.0f / resolution.z
      );
      return uv;
    }

    public readonly float GetValueAt(in int3 gridPos, in NativeArray<float> potential)
    {
      return GetValueAt(gridPos, Resolution, potential);
    }

    public static float GetValueAt(
      in int3 gridPos,
      in int3 resolution,
      in NativeArray<float> potential
    )
    {
      var idx = ConvertGridToIndex(gridPos, resolution);
      if (idx >= 0)
        return potential[idx];
      return 0;
    }

    public readonly float3 GetValueAt(in int3 gridPos, in NativeArray<float3> velocities)
    {
      return GetValueAt(gridPos, Resolution, velocities);
    }

    public static float3 GetValueAt(
      in int3 gridPos,
      in int3 resolution,
      in NativeArray<float3> velocities
    )
    {
      var idx = ConvertGridToIndex(gridPos, resolution);
      if (idx >= 0)
        return velocities[idx];
      return 0;
    }


    public readonly float3 FindGradientAt(in int idx, in NativeArray<float> potential)
    {
      return FindGradientAt(idx, Resolution, CellSize, potential);
    }

    public static float3 FindGradientAt(
      in int idx,
      in int3 resolution,
      in float3 cellSize,
      in NativeArray<float> potential
    )
    {
      var gridPos = ConvertIndexToGrid(idx, resolution);

      var grid1 = gridPos + new int3(1, 0, 0);
      var p1 = GetValueAt(grid1, resolution, potential);

      var grid2 = gridPos - new int3(1, 0, 0);
      var p2 = GetValueAt(grid2, resolution, potential);

      var grid3 = gridPos + new int3(0, 1, 0);
      var p3 = GetValueAt(grid3, resolution, potential);

      var grid4 = gridPos - new int3(0, 1, 0);
      var p4 = GetValueAt(grid4, resolution, potential);

      var nabla_p = Nabla(p1, p2, p3, p4, cellSize.x, cellSize.y);
      return new float3(nabla_p.x, nabla_p.y, 0);
    }

    public readonly float FindDivergenceAt(in int idx, in NativeArray<float3> velocities)
    {
      return FindDivergenceAt(idx, Resolution, CellSize, velocities);
    }

    public static float FindDivergenceAt(
      in int idx,
      in int3 resolution,
      in float3 cellSize,
      in NativeArray<float3> velocities
    )
    {
      var gridPos = ConvertIndexToGrid(idx, resolution);

      var grid1 = gridPos + new int3(1, 0, 0);
      var idx1 = ConvertGridToIndex(grid1, resolution);
      var v1 = new float2(velocities[idx1].x, velocities[idx1].y);

      var grid2 = gridPos - new int3(1, 0, 0);
      var idx2 = ConvertGridToIndex(grid2, resolution);
      var v2 = new float2(velocities[idx2].x, velocities[idx2].y);

      var grid3 = gridPos + new int3(0, 1, 0);
      var idx3 = ConvertGridToIndex(grid3, resolution);
      var v3 = new float2(velocities[idx3].x, velocities[idx3].y);

      var grid4 = gridPos - new int3(0, 1, 0);
      var idx4 = ConvertGridToIndex(grid4, resolution);
      var v4 = new float2(velocities[idx4].x, velocities[idx4].y);

      return Divergence(v1, v2, v3, v4, cellSize.x, cellSize.y);
    }

    public static float2 Nabla(
      in float rightNeighbor,
      in float leftNeighbor,
      in float upNeighbor,
      in float downNeighbor,
      in float hx = 1.0f,
      in float hy = 1.0f
    )
    {
      var dx = (rightNeighbor - leftNeighbor) / (2f * hx);
      var dy = (upNeighbor - downNeighbor) / (2f * hy);
      return new float2(dx, dy);
    }

    public static float Divergence(
      in float2 rightNeighbor,
      in float2 leftNeighbor,
      in float2 upNeighbor,
      in float2 downNeighbor,
      in float dx = 1.0f,
      in float dy = 1.0f
    )
    {
      var ddx = (rightNeighbor.x - leftNeighbor.x) / (2 * dx);
      var ddy = (upNeighbor.y - downNeighbor.y) / (2 * dy);
      return ddx + ddy;
    }
  }
}