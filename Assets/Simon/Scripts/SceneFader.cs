using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Graphic))]
public class SceneFader : MonoBehaviour
{

  // Configuration:
  public float fadeTime = 1;

  // Cache:
  private Graphic graphic;
  private bool hasFaded = false;

  // Messages:

  void Awake()
  {
    graphic = GetComponent<Graphic>();
    Color color = graphic.color;
    color.a = 1;
    graphic.color = color;
  }

  IEnumerator Start()
  {
    yield return HitchLib.Tweening.FadeTo(graphic, fadeTime, HitchLib.Easing.EASE_QUAD_IN, 0);
  }

  // Utilities:

  public void Fade(System.Action callback)
  {
    if(!hasFaded)
    {
      hasFaded = true;
      StartCoroutine(FadeWithCallback(callback));
    }
  }

  // Corutines:

  private IEnumerator FadeWithCallback(System.Action callback)
  {
    yield return HitchLib.Tweening.FadeTo(graphic, fadeTime, HitchLib.Easing.EASE_QUAD_IN, 1);
    callback();
  }

}
