using Unity.Entities;
using Unity.Mathematics;

public partial struct StaticGrid : IComponentData
{
  public readonly bool IsOutsideGridAt(in float3 rotatedPos, in float3x3 rotation)
  {
    return IsOutsideGridAt(rotatedPos, Origin, CellSize, Resolution, rotation);
  }

  public readonly int ConvertWorldToIndex(in float3 rotatedPos, in float3x3 rotation)
  {
    return ConvertWorldToIndex(rotatedPos, Origin, CellSize, Resolution, rotation);
  }

  public readonly int3 ConvertWorldToGrid(in float3 rotatedPos, in float3x3 rotation)
  {
    return ConvertWorldToGrid(rotatedPos, Origin, CellSize, rotation);
  }

  public readonly float3 ConvertIndexToWorld(in int index, in float3x3 rotation)
  {
    return ConvertIndexToWorld(index, Origin, CellSize, Resolution, rotation);
  }

  public readonly float3 ConvertGridToWorld(in int3 gridPos, in float3x3 rotation)
  {
    return ConvertGridToWorld(gridPos, Origin, CellSize, rotation);
  }

  public static bool IsOutsideGridAt(
    in float3 rotatedPos,
    in float3 origin,
    in float3 cellSize,
    in int3 resolution,
    in float3x3 rotation
  )
  {
    var gridPos = ConvertWorldToGrid(rotatedPos, origin, cellSize, rotation);
    return IsOutsideGridAt(gridPos, resolution);
  }

  public static float3 ConvertIndexToWorld(
    in int index,
    in float3 origin,
    in float3 cellSize,
    in int3 resolution,
    in float3x3 rotation
  )
  {
    var gridPos = ConvertIndexToGrid(index, resolution);
    return ConvertGridToWorld(gridPos, origin, cellSize, rotation);
  }

  public static int ConvertWorldToIndex(
    in float3 rotatedPos,
    in float3 origin,
    in float3 cellSize,
    in int3 resolution,
    in float3x3 rotation
  )
  {
    var gridPos = ConvertWorldToGrid(rotatedPos, origin, cellSize, rotation);
    return ConvertGridToIndex(gridPos, resolution);
  }

  public static float3 ConvertGridToWorld(
    in int3 gridPos,
    in float3 origin,
    in float3 cellSize,
    in float3x3 rotation
  )
  {
    var nonRotatedPos = ConvertGridToWorld(gridPos, origin, cellSize);
    return math.mul(rotation, nonRotatedPos - origin) + origin;
  }

  public static int3 ConvertWorldToGrid(
    in float3 rotatedPos,
    in float3 origin,
    in float3 cellSize,
    in float3x3 rotation
  )
  {
    var r = math.mul(math.transpose(rotation), rotatedPos - origin);
    return ConvertWorldToGrid(r + origin, origin, cellSize);
  }
}