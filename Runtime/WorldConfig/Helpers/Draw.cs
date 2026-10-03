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
      var color = GetColorFrom(colorIndex);
      color.a = colorAlpha;
      Debug.DrawLine(start, end, color, duration);
#endif
    }

    public static void DrawRay(
      float3 start,
      float3 dir,
      ColorIndex colorIndex = 0,
      float colorAlpha = 1.0f,
      float duration = 0f)
    {
#if UNITY_EDITOR
      var color = GetColorFrom(colorIndex);
      color.a = colorAlpha;
      Debug.DrawRay(start, dir, color, duration);
#endif
    }

    // Circle on the XY plane
    public static void DrawCircle(
      float3 center,
      float radius,
      ColorIndex colorIndex = 0,
      float colorAlpha = 1.0f,
      int segments = 24,
      float duration = 0f)
    {
#if UNITY_EDITOR
      var color = GetColorFrom(colorIndex);
      color.a = colorAlpha;
      if (segments < 3) segments = 3;
      if (radius <= 0f)
      {
        Debug.DrawLine(center, center + new float3(0f, 0.01f, 0f), color, duration);
        return;
      }

      var angleStep = 2f * math.PI / segments;
      var prev = center + new float3(radius, 0f, 0f);
      for (var i = 1; i <= segments; ++i)
      {
        var angle = angleStep * i;
        var next = center + new float3(
          math.cos(angle) * radius, math.sin(angle) * radius, 0f);
        Debug.DrawLine(prev, next, color, duration);
        prev = next;
      }
#endif
    }

    public static void DrawCircle(
      float2 center,
      float radius,
      ColorIndex colorIndex = 0,
      float colorAlpha = 1.0f,
      int segments = 24,
      float duration = 0f)
    {
#if UNITY_EDITOR
      DrawCircle(new float3(center.x, center.y, 0f),
        radius, colorIndex, colorAlpha, segments, duration);
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
      var color = GetColorFrom(colorIndex);
      color.a = colorAlpha;
      if (segments < 3) segments = 3;

      var n = math.normalizesafe(normal);
      if (radius <= 0f)
      {
        Debug.DrawLine(center, center + n * 0.01f, color, duration);
        return;
      }

      var tangent = math.cross(n, new float3(0f, 1f, 0f));
      if (math.lengthsq(tangent) < 1e-6f)
        tangent = math.cross(n, new float3(1f, 0f, 0f));
      tangent = math.normalizesafe(tangent);
      var bitangent = math.cross(n, tangent);

      var angleStep = 2f * math.PI / segments;
      var prev = center + tangent * radius;
      for (var i = 1; i <= segments; ++i)
      {
        var angle = angleStep * i;
        var next = center +
          (tangent * math.cos(angle) + bitangent * math.sin(angle)) * radius;
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
      float duration = 0f)
    {
#if UNITY_EDITOR
      if (radius <= 0f)
      {
        // FIX: colorAlpha was missing, so duration landed in the alpha slot
        DrawLine(center, center + new float3(0f, 0.01f, 0f),
          colorIndex, colorAlpha, duration);
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
      float duration = 0)
    {
#if UNITY_EDITOR
      DrawSphere(start, radius, colorIndex, colorAlpha, segments, duration);
      DrawSphere(end, radius * .5f, colorIndex, colorAlpha, segments, duration);
      DrawLine(start, end, colorIndex, colorAlpha, duration);
#endif
    }

    // Gizmos only work in OnDrawGizmos (managed, NOT Burst-safe)
    public static void GizmosDrawCircle(
      Vector3 center, float3 normal, float radius, int segments = 48)
    {
      var n = math.normalize(normal);
      BuildOrthonormalBasisesBy(n, out var t, out var b);

      var prev = center + (Vector3)(t * radius);
      for (var i = 1; i <= segments; ++i)
      {
        var angle = (float)i / segments * math.PI * 2f;
        var next = center +
          (Vector3)((t * math.cos(angle) + b * math.sin(angle)) * radius);
        Gizmos.DrawLine(prev, next);
        prev = next;
      }
    }

    // Gizmos only work in OnDrawGizmos (managed, NOT Burst-safe)
    public static void GizmosDrawCone(
      Vector3 origin, float3 dir, float halfAngleRad, float length, int segments = 32)
    {
      var n = math.normalize(dir);
      BuildOrthonormalBasisesBy(n, out var t, out var b);

      var tipOffset = (Vector3)(n * length);
      var ringRadius = length * math.tan(halfAngleRad);
      var ringCenter = origin + tipOffset;

      var prev = ringCenter + (Vector3)(t * ringRadius);
      for (var i = 1; i <= segments; ++i)
      {
        var angle = (float)i / segments * math.PI * 2f;
        var next = ringCenter +
          (Vector3)((t * math.cos(angle) + b * math.sin(angle)) * ringRadius);
        Gizmos.DrawLine(prev, next);
        prev = next;
      }

      for (var i = 0; i < 8; ++i)
      {
        var angle = (float)i / 8 * math.PI * 2f;
        var rim = ringCenter +
          (Vector3)((t * math.cos(angle) + b * math.sin(angle)) * ringRadius);
        Gizmos.DrawLine(origin, rim);
      }
    }

    // FIX: Burst-safe. No managed array, no local function, editor only.
    public static void DrawWireBox(
      float3 center,
      quaternion rotation,
      float3 size,
      Color color,
      float duration = 0f,
      bool depthTest = true)
    {
#if UNITY_EDITOR
      var h = size * 0.5f;
      for (var i = 0; i < 4; ++i)
      {
        var next = (i + 1) % 4;
        var a = BoxCorner(i, h, rotation, center);
        var b = BoxCorner(next, h, rotation, center);
        var aTop = BoxCorner(i + 4, h, rotation, center);
        var bTop = BoxCorner(next + 4, h, rotation, center);
        Debug.DrawLine(a, b, color, duration, depthTest);       // bottom edge
        Debug.DrawLine(aTop, bTop, color, duration, depthTest); // top edge
        Debug.DrawLine(a, aTop, color, duration, depthTest);    // vertical edge
      }
#endif
    }

#if UNITY_EDITOR
    // index 0-3 = bottom (clockwise), 4-7 = top
    static float3 BoxCorner(int index, float3 h, quaternion rotation, float3 center)
    {
      var i = index & 3;
      var x = (i == 1 || i == 2) ? h.x : -h.x;
      var z = (i == 2 || i == 3) ? h.z : -h.z;
      var y = index >= 4 ? h.y : -h.y;
      return math.rotate(rotation, new float3(x, y, z)) + center;
    }
#endif
  }
}