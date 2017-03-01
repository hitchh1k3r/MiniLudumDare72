using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectScaler : MonoBehaviour
{

  // Configure:
  public Camera cam;
  public float minAspect = 1.0f;
  public float minScale  = 1.0f;
  public float maxAspect = 2.0f;
  public float maxScale  = 0.5f;

  // Messages:

  void Update()
  {
    float scale = (cam.aspect - minAspect) / (maxAspect - minAspect);
    if(scale < 0)
    {
      scale = 0;
    }
    if(scale > 1)
    {
      scale = 1;
    }
    scale = ((1-scale) * minScale) + (scale * maxScale);
    transform.localScale = new Vector3(scale, scale, 1);
  }

}
