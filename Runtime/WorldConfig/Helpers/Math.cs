using Unity.Collections;
using Unity.Mathematics;

namespace HoangNam.WorldConfig
{
  public static partial class Helper
  {
    public static float ConvertDegToRad(float degrees)
    {
      return degrees * math.PI / 180f;
    }

    public static float3 MapRange(float3 value, float3 fromMin, float3 fromMax, float3 toMin, float3 toMax)
    {
      return toMin + (value - fromMin) * (toMax - toMin) / (fromMax - fromMin);
    }

    public static float MapRange(float value, float oldMin, float oldMax, float newMin, float newMax)
    {
      return newMin + (value - oldMin) * (newMax - newMin) / (oldMax - oldMin);
    }

    public static bool IsPointInLine(float3 givenPoint, float3 lineOrigin, float3 lineDirection)
    {
      /// Nr + d = 0 is a line equation with lineOrigin r and its direction v
      /// N = (-v.y, v.x) => d = -N * r
      var r = lineOrigin;
      var v = lineDirection;
      var N = new float3(-v.y, v.x, 0);
      var d = -math.dot(N, r);
      var val = math.dot(N, givenPoint) + d;
      if (val == 0) return true;
      return false;
    }

    public static float3 ProjectPointToLine(
      float3 givenPoint,
      float3 lineOrigin,
      float3 lineUnitDirection
    )
    {
      var r = givenPoint - lineOrigin;
      var n = new float3(-lineUnitDirection.y, lineUnitDirection.x, 0);
      float d = math.dot(r, n);
      return lineOrigin + (r - d * n);
    }


    public static float3 Lerp(float3 start, float3 end, float t) =>
      (1 - t) * start + t * end;

    public static void InterpolateMove(
      float3 startPos,
      float3 targetPos,
      float t,
      out float3 nextPos
    )
    {
      nextPos = Lerp(startPos, targetPos, t);
    }

    public static void InterpolateMoveInUpdate(
      float3 currentPos,
      float3 startPos,
      float3 targetPos,
      float speed,
      float dt,
      out float t,
      out float3 nextPos
    )
    {
      var distanceFromStart = math.length(currentPos - startPos);
      var totalDistance = math.length(targetPos - startPos);
      var _t = distanceFromStart / totalDistance + speed * dt / totalDistance;
      t = math.min(_t, 1);
      nextPos = Lerp(startPos, targetPos, t);
    }

    public static void InterpolatePathInUpdate(
      float3 currentPos,
      int currentIdx,
      int endIdx,
      NativeArray<float3> path,
      float speed,
      float dt,
      out float t,
      out float3 nextPos,
      out int nextIdx
    )
    {
      t = 1;
      nextPos = currentPos;
      nextIdx = currentIdx;
      var maxIdx = math.min(endIdx, path.Length - 1);
      if (currentIdx > maxIdx) return;

      var startPos = path[currentIdx];
      var targetIdx = currentIdx + 1;
      if (targetIdx > maxIdx) return;

      var targetPos = path[targetIdx];
      InterpolateMoveInUpdate(
        currentPos, startPos, targetPos, speed, dt,
        out var percent, out var nextPosition);
      t = (currentIdx + percent) / math.max(maxIdx, 1);
      nextPos = nextPosition;
      if (percent < 1)
      {
        nextIdx = currentIdx;
        return;
      }
      nextIdx = currentIdx + 1;
    }

    /// <summary>
    /// return basic axis directions in world space of an imagine object by a given quaternion
    /// </summary>
    public static void BuildOrthonormalBasisesBy(
      quaternion rotation,
      out float3 right,
      out float3 up,
      out float3 forward
    )
    {
      var worldRight = new float3(1, 0, 0);
      var worldUp = new float3(0, 1, 0);
      var worldForward = new float3(0, 0, 1);
      right = math.mul(rotation, worldRight);
      up = math.mul(rotation, worldUp);
      forward = math.mul(rotation, worldForward);
    }

    public static void BuildOrthonormalBasisesBy(
      float3 direction, out float3 right, out float3 up
    )
    {
      var _up = new float3(0, 1, 0);
      if (direction.y > .99f)
      {
        direction = _up;
        _up = new float3(0, 0, -1);
      }
      right = math.cross(direction, _up);
      up = math.cross(right, direction);
    }
  }
}