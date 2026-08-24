using Unity.Entities;
using Unity.Collections;

namespace HoangNam
{
  public struct CellWorld : IComponentData
  {
    public StaticGrid Grid;
    public NativeParallelMultiHashMap<int, TrackedData> CellIdxAndTrackeds;
    public NativeParallelHashMap<int, CellData> CellIdxes;
    public NativeList<int> OccupiedCellIdxes;

    public CellWorld(
      StaticGrid Grid,
      int potentialTrackedAmount,
      int potentialOccupiedCellAmount
    )
    {
      this.Grid = Grid;
      CellIdxAndTrackeds = new NativeParallelMultiHashMap<int, TrackedData>(
        potentialTrackedAmount, Allocator.Persistent);
      CellIdxes = new NativeParallelHashMap<int, CellData>(
        potentialOccupiedCellAmount, Allocator.Persistent);
      OccupiedCellIdxes = new NativeList<int>(
        potentialOccupiedCellAmount, Allocator.Persistent
      );
    }

    public void SetCapacity(
      int potentialTrackedAmount,
      int potentialOccupiedCellAmount
    )
    {
      if (CellIdxAndTrackeds.Capacity < potentialTrackedAmount)
        CellIdxAndTrackeds.Capacity = potentialTrackedAmount;
      if (CellIdxes.Capacity < potentialOccupiedCellAmount)
        CellIdxes.Capacity = potentialOccupiedCellAmount;
      if (OccupiedCellIdxes.Capacity < potentialOccupiedCellAmount)
        OccupiedCellIdxes.Capacity = potentialOccupiedCellAmount;
    }

    public void Clear()
    {
      CellIdxes.Clear();
      OccupiedCellIdxes.Clear();
      CellIdxAndTrackeds.Clear();
    }

    public void Dispose()
    {
      if (CellIdxes.IsCreated) CellIdxes.Dispose();
      if (OccupiedCellIdxes.IsCreated) OccupiedCellIdxes.Dispose();
      if (CellIdxAndTrackeds.IsCreated) CellIdxAndTrackeds.Dispose();
    }
  }
}