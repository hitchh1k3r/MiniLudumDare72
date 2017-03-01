using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Graphic))]
public class Popup : MonoBehaviour
{

  // Configure:
  public float inTime = 0.125f;
  public float hangTime = 0.333333f;
  public float outTime = 0.25f;

  // Messages:

  IEnumerator Start()
  {
    Graphic pop = GetComponent<Graphic>();
    Color c = pop.color;
    yield return HitchLib.Tweening.GenericTween(t => {
            c.a = t;
            pop.color = c;
            transform.localScale = new Vector3(t * 0.8f, t * 0.8f, 1);
          }, 0.0f, 1.0f, inTime, HitchLib.Easing.EASE_QUAD_OUT);
    yield return HitchLib.Tweening.GenericTween(t => {
            transform.localScale = new Vector3(0.8f + (t * 0.2f), 0.8f + (t * 0.2f), 1);
          }, 0.0f, 1.0f, hangTime, HitchLib.Easing.EASE_LINEAR);
    yield return HitchLib.Tweening.GenericTween(t => {
            c.a = 1 - t;
            pop.color = c;
            transform.localScale = new Vector3(1 + (t * 0.5f), 1 + (t * 0.5f), 1);
          }, 0.0f, 1.0f, outTime, HitchLib.Easing.EASE_QUAD_IN);
    Destroy(gameObject);
  }

}
