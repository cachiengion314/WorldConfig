using UnityEngine;

namespace HoangNam.WorldConfig
{
  [CreateAssetMenu(fileName = "Config", menuName = "World Config/Config")]
  public class WorldConfigObj : ScriptableObject
  {
    [ViewOnly]
    public float SimulatedSpeed = 1;

    [Range(.0001f, 1.0f)]
    public float NormalSimulatedSpeed = 1.0f;
    [Range(.0001f, 1.0f)]
    public float SlowdownSimulatedSpeed = .125f;
  }
}