using Unity.Mathematics;

namespace HoangNam
{
  public static partial class Helper
  {
    /// <summary>
    /// give a random direction in a cone that have a defined spread angle
    /// plus its main axis defined by a unit preferred direction
    /// </summary>
    /// <param name="preferredDirection"></param>
    /// <param name="spreadAngle"></param>
    /// <param name="rand"></param>
    /// <returns></returns>
    public static float3 GetRandomDirection(
      float3 preferredDirection, float spreadAngle, ref Random rand
    )
    {
      var alpha = ConvertDegToRad(spreadAngle);
      var cosAlpha = math.cos(alpha);
      var cosTheta = rand.NextFloat(cosAlpha, 1);
      var phi = rand.NextFloat(0, math.PI2);
      var z = cosTheta;
      var sinTheta = math.sqrt(1 - cosTheta * cosTheta);
      var x = sinTheta * math.cos(phi);
      var y = sinTheta * math.sin(phi);
      var v1 = new float3(x, y, z);

      BuildOrthonormalBasisesBy(preferredDirection,
        out var right, out var up);
      var v2 = v1.x * right + v1.y * up + v1.z * preferredDirection;

      return v2;
    }

    /// <summary>
    /// give a random direction in a plane where n is its unit normal vector
    /// </summary>
    /// <param name="n"></param>
    /// <param name="random"></param>
    /// <returns></returns>
    public static float3 GetRandomDirection(float3 n, ref Random rand)
    {
      var phi = rand.NextFloat(0, math.PI2);
      var v = new float3(math.cos(phi), math.sin(phi), 0);
      BuildOrthonormalBasisesBy(n, out var right, out var up);
      return v.x * right + v.y * up;
    }

    /// <summary>
    /// give a random direction that have its point place in area of an unit sphere
    /// </summary>
    /// <param name="rand"></param>
    /// <returns></returns>
    public static float3 GetRandomDirection(ref Random rand)
    {
      var cosTheta = rand.NextFloat(-1, 1);
      var phi = rand.NextFloat(0, math.PI2);
      var z = cosTheta;
      var sinTheta = math.sqrt(1 - cosTheta * cosTheta);
      var x = sinTheta * math.cos(phi);
      var y = sinTheta * math.sin(phi);
      return new float3(x, y, z);
    }
  }
}