using UnityEngine;
using Unity.Entities;

public class WorldConfigAuthoring : MonoBehaviour
{
  [Header("World config section")]
  public WorldConfigObj WorldConfigObj;

  class Baker : Baker<WorldConfigAuthoring>
  {
    public override void Bake(WorldConfigAuthoring authoring)
    {
      if (authoring.WorldConfigObj == null) return;

      var e = GetEntity(TransformUsageFlags.None);

      AddComponent(
        e,
        new WorldConfig
        {
          NormalSimulatedSpeed = authoring.WorldConfigObj.NormalSimulatedSpeed,
          SlowdownSimulatedSpeed = authoring.WorldConfigObj.SlowdownSimulatedSpeed
        }
      );
      AddComponent(
        e,
        new WorldData
        {
          SimulatedSpeed = authoring.WorldConfigObj.NormalSimulatedSpeed,
        }
      );
    }
  }
}
