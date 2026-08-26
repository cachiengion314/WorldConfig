using Unity.Entities;

namespace HoangNam.WorldConfig
{
  public struct CellData : IComponentData
  {
    public int Index;
    public int Count;
    public int CombinedMask;
  }

  public struct TrackedData
  {
    public Entity Entity;
    public int Mask;
  }
}