using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundBounce : MonoBehaviour
{

  // Configure:
  public Camera cam;
  public float moveTime = 4.0f;

  // State:
  private float aspect;
  private float size;
  private Vector3 pos = new Vector3(0, 0, 10);

  // Messages:

  IEnumerator Start()
  {
    while(true)
    {
      Vector3 target = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 10);
      float dist = (target - pos).magnitude + 1;
      yield return HitchLib.Tweening.GenericTween(v => { pos = v; }, pos, target, moveTime * dist,
            HitchLib.Easing.EASE_QUAD_IN_OUT);
    }
  }

  void Update()
  {
    if(aspect < cam.aspect - 0.0001f || aspect > cam.aspect + 0.0001f)
    {
      aspect = cam.aspect;
      size = 20;
      if(aspect > 1)
      {
        size *= aspect;
      }
      transform.localScale = new Vector3(size, size, 1);
    }

    transform.localPosition = (Vector3)(size * 0.25f * pos) + Vector3.forward;
  }

}
