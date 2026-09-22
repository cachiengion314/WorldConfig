using Unity.Entities;

namespace HoangNam.WorldConfig
{
  [UpdateInGroup(typeof(InitializationSystemGroup))]
  public partial class WritableSystemGroup : ComponentSystemGroup { }

  [UpdateInGroup(typeof(SimulationSystemGroup))]
  public partial class ReadOnlySystemGroup : ComponentSystemGroup { }

  [UpdateInGroup(typeof(PresentationSystemGroup))]
  public partial class CleanUpSystemGroup : ComponentSystemGroup { }
}