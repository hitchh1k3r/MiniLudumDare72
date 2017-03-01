using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchScale : MonoBehaviour
{

  void Update()
  {
    Vector3 scale = transform.localScale;
    scale.y = GlobalSingleton.GetTimelineScale();
    transform.localScale = scale;
  }

}
