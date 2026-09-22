using Unity.Burst;
using Unity.Entities;

namespace HoangNam.WorldConfig
{
  [UpdateInGroup(typeof(WritableSystemGroup), OrderFirst = true)]
  public partial struct WorldConfigSystem : ISystem
  {
    public void OnCreate(ref SystemState state)
    {
      state.RequireForUpdate<WorldConfig>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
      var worldData = SystemAPI.GetSingletonRW<WorldData>();
      var dt = worldData.ValueRO.SimulatedSpeed * SystemAPI.Time.DeltaTime;

      worldData.ValueRW.DeltaTime = dt;
      worldData.ValueRW.ElapsedTime = worldData.ValueRO.ElapsedTime + dt;
      worldData.ValueRW.UnscaledDeltaTime = SystemAPI.Time.DeltaTime;
      worldData.ValueRW.UnscaledElapsedTime = SystemAPI.Time.ElapsedTime;
    }
  }
}
