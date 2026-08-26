using UnityEngine;
using Unity.Entities;

namespace HoangNam.WorldConfig
{
  public partial class CellWorldAuthoring : MonoBehaviour
  {
    public StaticGrid Grid;

    class Baker : Baker<CellWorldAuthoring>
    {
      public override void Bake(CellWorldAuthoring authoring)
      {
        var e = GetEntity(TransformUsageFlags.None);
        AddComponent(e, new StaticGrid
        {
          CellSize = authoring.Grid.CellSize,
          Origin = authoring.Grid.Origin,
          Resolution = authoring.Grid.Resolution,
        });
      }
    }
  }
}