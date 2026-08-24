using UnityEngine;

public class FrameRateLimiter : MonoBehaviour
{
  [SerializeField] bool UseInEditor;
  [Range(1, 120)]
  [SerializeField] int TargetFrameRate = 60;

  void Awake()
  {
#if UNITY_EDITOR
    if (!UseInEditor)
      return;
#endif

    Application.targetFrameRate = TargetFrameRate;
    QualitySettings.vSyncCount = 0;
  }
}