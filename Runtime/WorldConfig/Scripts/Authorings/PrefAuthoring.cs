using UnityEngine;
using Unity.Entities;

public class PrefAuthoring : MonoBehaviour
{
  class Baker : Baker<PrefAuthoring>
  {
    public override void Bake(PrefAuthoring authoring)
    {
      var entity = GetEntity(TransformUsageFlags.Dynamic);

      AddComponent(entity, new LinkedParent { Value = Entity.Null });
      AddComponent(entity, new LinkedGrandParent { Value = Entity.Null });
      AddComponent<CanRun>(entity);
      SetComponentEnabled<CanRun>(entity, true);
    }
  }
}