using System;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public partial struct StaticGrid : IComponentData
{
  public static readonly float MIN_CELLSIZE = .01f;

  public int3 Resolution;
  public float3 CellSize;
  public float3 Origin;
  public float3x3 Rotation;

  public readonly int GetCellCount()
  {
    return Resolution.x * Resolution.y * Resolution.z;
  }

  public readonly bool IsOutsideGridAt(int index)
  {
    return IsOutsideGridAt(index, Resolution);
  }

  public readonly bool IsOutsideGridAt(float3 position)
  {
    return IsOutsideGridAt(position, Origin, CellSize, Resolution);
  }

  public readonly bool IsOutsideGridAt(int3 gridPos)
  {
    return IsOutsideGridAt(gridPos, Resolution);
  }

  public readonly int3 ConvertWorldToGrid(float3 worldPos)
  {
    return ConvertWorldToGrid(worldPos, Origin, CellSize);
  }

  public readonly float3 ConvertGridToWorld(int3 gridPos)
  {
    return ConvertGridToWorld(gridPos, Origin, CellSize);
  }

  public readonly int ConvertGridToIndex(int3 gridPos)
  {
    return ConvertGridToIndex(gridPos, Resolution);
  }

  public readonly int3 ConvertIndexToGrid(int index)
  {
    return ConvertIndexToGrid(index, Resolution);
  }

  public readonly float3 ConvertIndexToWorld(int index)
  {
    return ConvertIndexToWorld(index, Origin, CellSize, Resolution);
  }

  public readonly int ConvertWorldToIndex(float3 position)
  {
    return ConvertWorldToIndex(position, Origin, CellSize, Resolution);
  }

  public static bool IsOutsideGridAt(
    in int index,
    in int3 resolution
  )
  {
    var gridPos = ConvertIndexToGrid(index, resolution);
    return IsOutsideGridAt(gridPos, resolution);
  }

  public static bool IsOutsideGridAt(
    in float3 position,
    in float3 origin,
    in float3 cellSize,
    in int3 resolution
  )
  {
    var gridPos = ConvertWorldToGrid(position, origin, cellSize);
    return IsOutsideGridAt(gridPos, resolution);
  }

  public static bool IsOutsideGridAt(
    in int3 gridPos,
    in int3 resolution
  )
  {
    return
      gridPos.x < 0 ||
      gridPos.y < 0 ||
      gridPos.z < 0 ||
      gridPos.x >= resolution.x ||
      gridPos.y >= resolution.y ||
      gridPos.z >= resolution.z;
  }

  public static int3 ConvertWorldToGrid(
    in float3 worldPos,
    in float3 origin,
    in float3 cellSize
  )
  {
    var _cellSize = math.max(MIN_CELLSIZE, cellSize);
    return (int3)math.floor((worldPos - origin) / _cellSize);
  }

  public static float3 ConvertGridToWorld(
    in int3 gridPos,
    in float3 origin,
    in float3 cellSize
  )
  {
    var _cellSize = math.max(MIN_CELLSIZE, cellSize);
    return origin +
      (float3)gridPos * _cellSize +
      .5f * _cellSize;
  }

  public static int ConvertGridToIndex(
    in int3 gridPos,
    in int3 resolution
  )
  {
    if (IsOutsideGridAt(gridPos, resolution)) return -1;
    var width = math.max(1, resolution.x);
    var height = math.max(1, resolution.y);
    var index = gridPos.x +
      (gridPos.y * width) +
      (gridPos.z * width * height);
    return index;
  }

  /// <summary>
  /// Can consider index as the volume of the shape
  /// </summary>
  public static int3 ConvertIndexToGrid(
    in int index,
    in int3 resolution
  )
  {
    var width = math.max(1, resolution.x);
    var height = math.max(1, resolution.y);
    var sliceArea = width * height;
    var remainArea = index % sliceArea;
    var z = index / sliceArea;
    var y = remainArea / width;
    var x = remainArea % width;
    return new int3(x, y, z);
  }

  public static float3 ConvertIndexToWorld(
    in int index,
    in float3 origin,
    in float3 cellSize,
    in int3 resolution
  )
  {
    var gridPos = ConvertIndexToGrid(index, resolution);
    return ConvertGridToWorld(gridPos, origin, cellSize);
  }

  public static int ConvertWorldToIndex(
    in float3 position,
    in float3 origin,
    in float3 cellSize,
    in int3 resolution
  )
  {
    var gridPos = ConvertWorldToGrid(position, origin, cellSize);
    return ConvertGridToIndex(gridPos, resolution);
  }

  public static int Hash(in int3 gridPos)
  {
    return (gridPos.x * 73856093) ^
           (gridPos.y * 19349663) ^
           (gridPos.z * 83492791);
  }

  public static float3 GetSpatialScale(
    in int3 originalResolution,
    in float3 originalScale,
    in int3 spatialResolution
  )
  {
    var xUnitAmount = (float)originalResolution.x / math.max(spatialResolution.x, 1);
    var yUnitAmount = (float)originalResolution.y / math.max(spatialResolution.y, 1);
    var zUnitAmount = (float)originalResolution.z / math.max(spatialResolution.z, 1);
    var xScale = xUnitAmount * originalScale.x;
    var yScale = yUnitAmount * originalScale.y;
    var zScale = zUnitAmount * originalScale.z;
    return new float3(xScale, yScale, zScale);
  }

  static int2 GetOriginalInterval(
    in int spatialGridPos,
    in int spatialRes,
    in int originalRes
  )
  {
    int start = spatialGridPos * originalRes / spatialRes;
    int end = (spatialGridPos + 1) * originalRes / spatialRes;
    return new int2(start, end);
  }

  public static void CollectOriginalGridPosAt(
    in int spatialCellIdx,
    in int3 spatialResolution,
    in int3 originalResolution,
    out int2 xOriginalInterval,
    out int2 yOriginalInterval,
    out int2 zOriginalInterval
  )
  {
    var spatialGridPos = ConvertIndexToGrid(spatialCellIdx, spatialResolution);

    xOriginalInterval = GetOriginalInterval(
      spatialGridPos.x, spatialResolution.x, originalResolution.x);
    yOriginalInterval = GetOriginalInterval(
      spatialGridPos.y, spatialResolution.y, originalResolution.y);
    zOriginalInterval = GetOriginalInterval(
      spatialGridPos.z, spatialResolution.z, originalResolution.z);
  }
}