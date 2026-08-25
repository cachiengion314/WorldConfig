using UnityEngine;
using Unity.Mathematics;

namespace HoangNam.WorldConfig
{
  public static partial class Helper
  {
    public static void DrawLine(
      float3 start,
      float3 end,
      ColorIndex colorIndex = 0,
      float colorAlpha = 1.0f,
      float duration = .0f)
    {
#if UNITY_EDITOR
      var color = Helper.GetColorFrom(colorIndex);
      color.a = colorAlpha;
      Debug.DrawLine(start, end, color, duration);
#endif
    }

    public static void DrawRay(
      float3 start,
      float3 dir,
      ColorIndex colorIndex = 0,
      float colorAlpha = 1.0f,
      float duration = 0f
    )
    {
#if UNITY_EDITOR
      var color = Helper.GetColorFrom(colorIndex);
      color.a = colorAlpha;
      Debug.DrawRay(start, dir, color, duration);
#endif
    }

    public static void DrawCircle(
      float3 center,
      float radius,
      ColorIndex colorIndex = 0,
      float colorAlpha = 1.0f,
      int segments = 24,
      float duration = 0f)
    {
#if UNITY_EDITOR
      var color = Helper.GetColorFrom(colorIndex);
      color.a = colorAlpha;
      if (segments < 3) segments = 3;
      if (radius <= 0f)
      {
        Debug.DrawLine(center, center + new float3(0f, 0.01f, 0f), color, duration);
        return;
      }

      float angleStep = 2f * math.PI / segments;
      float3 prev = center + new float3(radius, 0f, 0f);
      for (int i = 1; i <= segments; ++i)
      {
        float angle = angleStep * i;
        float3 next = center + new float3(math.cos(angle) * radius, math.sin(angle) * radius, 0f);
        Debug.DrawLine(prev, next, color, duration);
        prev = next;
      }
#endif
    }

    public static void DrawCircle(
      float2 center,
      float radius,
      ColorIndex colorIndex = 0,
      float colorALpha = 1.0f,
      int segments = 24,
      float duration = 0f)
    {
#if UNITY_EDITOR
      DrawCircle(new float3(center.x, center.y, 0f),
        radius, colorIndex, colorALpha, segments, duration);
#endif
    }

    public static void DrawCircle(
      float3 center,
      float radius,
      float3 normal,
      ColorIndex colorIndex = 0,
      float colorAlpha = 1.0f,
      int segments = 24,
      float duration = 0f)
    {
#if UNITY_EDITOR
      var color = Helper.GetColorFrom(colorIndex);
      color.a = colorAlpha;
      if (segments < 3) segments = 3;

      float3 n = math.normalizesafe(normal);
      if (radius <= 0f)
      {
        Debug.DrawLine(center, center + n * 0.01f, color, duration);
        return;
      }

      float3 tangent = math.cross(n, new float3(0f, 1f, 0f));
      if (math.lengthsq(tangent) < 1e-6f)
        tangent = math.cross(n, new float3(1f, 0f, 0f));
      tangent = math.normalizesafe(tangent);
      float3 bitangent = math.cross(n, tangent);

      float angleStep = 2f * math.PI / segments;
      float3 prev = center + (tangent * math.cos(0f) + bitangent * math.sin(0f)) * radius;
      for (int i = 1; i <= segments; ++i)
      {
        float angle = angleStep * i;
        float3 next = center + (tangent * math.cos(angle) + bitangent * math.sin(angle)) * radius;
        Debug.DrawLine(prev, next, color, duration);
        prev = next;
      }
#endif
    }

    public static void DrawSphere(
      float3 center,
      float radius,
      ColorIndex colorIndex = 0,
      float colorAlpha = 1.0f,
      int segments = 24,
      float duration = 0f
    )
    {
#if UNITY_EDITOR
      if (radius <= 0f)
      {
        DrawLine(center, center + new float3(0f, 0.01f, 0f), colorIndex, duration);
        return;
      }
      // XY plane
      DrawCircle(center, radius,
        new float3(0f, 0f, 1f), colorIndex, colorAlpha, segments, duration);
      // XZ plane
      DrawCircle(center, radius,
        new float3(0f, 1f, 0f), colorIndex, colorAlpha, segments, duration);
      // YZ plane
      DrawCircle(center, radius,
        new float3(1f, 0f, 0f), colorIndex, colorAlpha, segments, duration);
#endif
    }

    public static void DrawHighlightLine(
      float3 start,
      float3 end,
      float radius,
      ColorIndex colorIndex = ColorIndex.None,
      float colorAlpha = 1.0f,
      int segments = 24,
      float duration = 0
    )
    {
      DrawSphere(start, radius, colorIndex, colorAlpha, segments, duration);
      DrawSphere(end, radius * .5f, colorIndex, colorAlpha, segments, duration);
      DrawLine(start, end, colorIndex, colorAlpha, duration);
    }

    /// <summary>
    /// only work in OnDrawGizmos
    /// </summary>
    public static void GizmosDrawCircle(
      Vector3 center, float3 normal, float radius, int segments = 48)
    {
      var n = math.normalize(normal);
      BuildOrthonormalBasisesBy(n, out var t, out var b);

      var prev = center + (Vector3)(t * radius);
      for (int i = 1; i <= segments; ++i)
      {
        var angle = (float)i / segments * math.PI * 2f;
        var next = center + (Vector3)((t * math.cos(angle) + b * math.sin(angle)) * radius);
        Gizmos.DrawLine(prev, next);
        prev = next;
      }
    }

    /// <summary>
    /// only work in OnDrawGizmos
    /// </summary>
    public static void GizmosDrawCone(
      Vector3 origin, float3 dir, float halfAngleRad, float length, int segments = 32)
    {
      var n = math.normalize(dir);
      BuildOrthonormalBasisesBy(n, out var t, out var b);

      var tipOffset = (Vector3)(n * length);
      var ringRadius = length * math.tan(halfAngleRad);
      var ringCenter = origin + tipOffset;

      var prev = ringCenter + (Vector3)(t * ringRadius);
      for (int i = 1; i <= segments; ++i)
      {
        var angle = (float)i / segments * math.PI * 2f;
        var next = ringCenter + (Vector3)((t * math.cos(angle) + b * math.sin(angle)) * ringRadius);
        Gizmos.DrawLine(prev, next);
        prev = next;
      }

      for (int i = 0; i < 8; ++i)
      {
        var angle = (float)i / 8 * math.PI * 2f;
        var rim = ringCenter + (Vector3)((t * math.cos(angle) + b * math.sin(angle)) * ringRadius);
        Gizmos.DrawLine(origin, rim);
      }
    }

    public static void DrawWireBox(
      float3 center,
      quaternion rotation,
      float3 size,
      Color color,
      float duration = 0f,
      bool depthTest = true
    )
    {
      float3 h = size * 0.5f;

      float3[] p =
      {
      new(-h.x,-h.y,-h.z),
      new( h.x,-h.y,-h.z),
      new( h.x,-h.y, h.z),
      new(-h.x,-h.y, h.z),

      new(-h.x, h.y,-h.z),
      new( h.x, h.y,-h.z),
      new( h.x, h.y, h.z),
      new(-h.x, h.y, h.z)
      };

      for (int i = 0; i < 8; i++)
        p[i] = math.rotate(rotation, p[i]) + center;

      DrawEdge(0, 1); DrawEdge(1, 2); DrawEdge(2, 3); DrawEdge(3, 0);
      DrawEdge(4, 5); DrawEdge(5, 6); DrawEdge(6, 7); DrawEdge(7, 4);
      DrawEdge(0, 4); DrawEdge(1, 5); DrawEdge(2, 6); DrawEdge(3, 7);

      void DrawEdge(int a, int b)
      {
        Debug.DrawLine(p[a], p[b], color, duration, depthTest);
      }
    }
  }
}