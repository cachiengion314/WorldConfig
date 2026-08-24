using System;
using UnityEngine;
using Unity.Mathematics;

public enum ColorIndex
{
  None,
  White,
  Black,
  Gray,
  Blue,
  Red,
  Green,
  Yellow,
  Cyan,
  Magenta,
  Orange,
  Lime,
  Purple,
  Pink,
  Azure,
  Beige,
  Chocolate,
  Coral
}

namespace HoangNam
{
  public static partial class Helper
  {
    public static void Print(in object o)
    {
#if DEBUG
      Debug.Log(o);
#endif
    }

    public static void Break()
    {
#if DEBUG
      Debug.Break();
#endif
    }

    public static float4 GetRandomColor(ref Unity.Mathematics.Random random)
    {
      var hue = (random.NextFloat() + 0.618034005f) % 1;
      return (Vector4)Color.HSVToRGB(hue, 1.0f, 1.0f);
    }

    public static Color GetColorFrom(in ColorIndex colorIndex = 0)
    {
      return colorIndex switch
      {
        ColorIndex.Black => Color.black,
        ColorIndex.Gray => Color.gray,
        ColorIndex.Blue => Color.blue,
        ColorIndex.Red => Color.red,
        ColorIndex.Green => Color.green,
        ColorIndex.Yellow => Color.yellow,
        ColorIndex.Cyan => Color.cyan,
        ColorIndex.Magenta => Color.magenta,
        ColorIndex.Orange => new Color(1.0f, 0.55f, 0.0f),
        ColorIndex.Lime => new Color(0.5f, 1.0f, 0.0f),
        ColorIndex.Purple => new Color(0.6f, 0.3f, 0.8f),
        ColorIndex.Pink => new Color(1.0f, 0.41f, 0.71f),
        ColorIndex.Azure => new Color(0.0f, 0.5f, 1.0f),
        ColorIndex.Beige => new Color(0.96f, 0.96f, 0.86f),
        ColorIndex.Chocolate => new Color(0.48f, 0.25f, 0.0f),
        ColorIndex.Coral => new Color(1.0f, 0.5f, 0.31f),
        _ => Color.white,
      };
    }

    public static int GetEpochMilliseconds() =>
      (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;

    public static int FindGreatestCommonDivisor(int a, int b)
    {
      var smaller = math.min(a, b);
      var bigger = math.max(a, b);
      var remain = bigger % smaller;
      while (remain > 0)
      {
        bigger = smaller;
        smaller = remain;
        remain = bigger % smaller;
      }
      return smaller;
    }
  }
}