using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

/// SystemAPI.GetSingleton<T>() caches the underlying query under the hood. 
/// It only looks up the entity once and retains a direct reference.
/// EntityManager.CreateEntityQuery(...) (as used in the extension method example) 
/// creates a new query object or has to look it up in a dictionary every single time 
/// it is called. Doing this repeatedly inside an OnUpdate loop creates a bottleneck 
/// and generates GC allocations in the editor
public static class SystemStateExtensions
{
  public static void Print(ref this SystemState state, object msg)
  {
#if UNITY_EDITOR
    Debug.Log(msg);
#endif
  }

  public static void PrintLine(
    ref this SystemState state,
    float3 start,
    float3 end,
    ColorIndex colorIndex,
    float colorAlpha = 1.0f,
    float duration = 0
  )
  {
#if UNITY_EDITOR
    HoangNam.Helper.DrawLine(start, end, colorIndex, colorAlpha, duration);
#endif
  }

  public static void PrintSphere(
    ref this SystemState state,
    float3 center,
    float radius,
    ColorIndex colorIndex,
    float colorAlpha = 1.0f,
    int segments = 24,
    float duration = 0
  )
  {
#if UNITY_EDITOR
    HoangNam.Helper.DrawSphere(
      center, radius, colorIndex, colorAlpha, segments, duration);
#endif
  }

  /// <summary>
  /// The function itself borrow EntityManager to gain debug draw ability
  /// so it will only run on main thread and will not burst compile compatible.
  /// </summary>
  public static void PrintWiredBoxAt(
    ref this SystemState state,
    float3 center,
    quaternion rotation,
    float3 size,
    ColorIndex colorIdx,
    float colorAlpha = 1.0f,
    float duration = .0f
  )
  {
#if UNITY_EDITOR
    var color = HoangNam.Helper.GetColorFrom(colorIdx);
    color.a = colorAlpha;
    HoangNam.Helper.DrawWireBox(
       center,
       rotation,
       size,
       color);
#endif
  }
}