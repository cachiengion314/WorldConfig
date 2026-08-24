using Unity.Entities;
using Unity.Mathematics;

/// <summary>
/// Global world settings.
/// Usually exists exactly once.
/// If multiple exist, the universe may disagree with itself.
/// </summary>
public struct WorldConfig : IComponentData
{
  public float NormalSimulatedSpeed;
  public float SlowdownSimulatedSpeed;
}

/// <summary>
/// Runtime world state.
/// Stores the current simulation speed and the delta time
/// that every system is probably secretly looking for.
/// </summary>
public struct WorldData : IComponentData
{
  public float SimulatedSpeed;
  public float UnscaledDeltaTime;
  public float DeltaTime;
}

/// <summary>
/// Requests a one-time trigger.
/// Like pressing a button, except the button is an entity.
/// </summary>
public struct CanTrigger : IComponentData, IEnableableComponent { }

/// <summary>
/// Requests spawning of something.
/// What gets spawned is somebody else's problem.
/// </summary>
public struct CanSpawn : IComponentData, IEnableableComponent { }

/// <summary>
/// Requests spawning again.
/// Because sometimes once is not enough.
/// </summary>
public struct CanReSpawn : IComponentData, IEnableableComponent { }

/// <summary>
/// Allows an entity to actively participate in simulation.
/// Disabled entities are effectively on vacation.
/// </summary>
public struct CanRun : IComponentData, IEnableableComponent { }

/// <summary>
/// Direct link to a parent entity.
/// Family trees sold separately.
/// </summary>
public struct LinkedParent : IComponentData
{
  public Entity Value;
}

/// <summary>
/// Direct link to a grandparent entity.
/// Useful when your parent is too busy being a parent.
/// </summary>
public struct LinkedGrandParent : IComponentData
{
  public Entity Value;
}

/// <summary>
/// Points to the entity responsible for ordering,
/// spawning, requesting, or generally causing something to happen.
/// The culprit, if you will.
/// </summary>
public struct LinkedOrderer : IComponentData
{
  public Entity Value;
}

/// <summary>
/// A request to spawn something at a position.
/// Contains optional user-defined metadata through LocalID.
/// The spawn system promises to carry it around.
/// It makes no promises about understanding it.
/// </summary>
public struct SpawnData : IBufferElementData
{
  /// <summary>
  /// User-defined identifier.
  /// May represent an event index, request handle,
  /// secret handshake, or anything else.
  /// Interpretation is left entirely to the caller.
  /// </summary>
  public int LocalID;
  public Entity Orderer;
  public float3 Position;
  public quaternion Rotation;
}

/// <summary>
/// User-defined identifier attached to an entity.
/// Meaning is determined entirely by whoever put it there.
/// Future archaeologists may have to guess.
/// </summary>
public struct LocalID : IComponentData
{
  public int Value;
}