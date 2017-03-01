using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{

  // Configuration:
  public Graphic heart_0, heart_1, heart_2, heart_broken_0, heart_broken_1, heart_broken_2;
  public Shake heart_shake_0, heart_shake_1, heart_shake_2;
  public Color healthyColor, hurtColor;

  // Cahce:
  private int health = 3;

  // State:
  private Coroutine aniCor;

  // Messages:

  void Update()
  {
    int hp = GlobalSingleton.GetHealth();
    if(hp != health)
    {
      health = hp;
      if(aniCor != null)
      {
        StopCoroutine(aniCor);
      }
      aniCor = StartCoroutine(UpdateHearts());
    }
  }

  // Corutines:

  private IEnumerator UpdateHearts()
  {
    Color h0S  = heart_0.color;
    Color h0bS = heart_broken_0.color;
    Color h0E  = Color.black;
    Color h0bE = hurtColor;
    if(health >= 1)
    {
      h0E = healthyColor;
      h0bE.a = 0;
    }
    if(h0bS.a > h0bE.a + 0.001f || h0bS.a < h0bE.a - 0.001f)
    {
      if(health >= 1)
      {
        heart_shake_0.StartShake(1.0f, 0.5f);
      }
      else
      {
        heart_shake_0.StartShake(0.15f, 0.33333f);
      }
    }
    Color h1S  = heart_1.color;
    Color h1bS = heart_broken_1.color;
    Color h1E  = Color.black;
    Color h1bE = hurtColor;
    if(health >= 2)
    {
      h1E = healthyColor;
      h1bE.a = 0;
    }
    if(h1bS.a > h1bE.a + 0.001f || h1bS.a < h1bE.a - 0.001f)
    {
      if(health >= 2)
      {
        heart_shake_1.StartShake(1.0f, 0.5f);
      }
      else
      {
        heart_shake_1.StartShake(0.15f, 0.33333f);
      }
    }
    Color h2S  = heart_2.color;
    Color h2bS = heart_broken_2.color;
    Color h2E  = Color.black;
    Color h2bE = hurtColor;
    if(health >= 3)
    {
      h2E = healthyColor;
      h2bE.a = 0;
    }
    if(h2bS.a > h2bE.a + 0.001f || h2bS.a < h2bE.a - 0.001f)
    {
      if(health >= 3)
      {
        heart_shake_2.StartShake(1.0f, 0.5f);
      }
      else
      {
        heart_shake_2.StartShake(0.15f, 0.33333f);
      }
    }

    yield return HitchLib.Tweening.GenericTween(t => {
            float it = 1 - t;
            heart_0.color        = it * h0S  + t * h0E;
            heart_broken_0.color = it * h0bS + t * h0bE;
            heart_1.color        = it * h1S  + t * h1E;
            heart_broken_1.color = it * h1bS + t * h1bE;
            heart_2.color        = it * h2S  + t * h2E;
            heart_broken_2.color = it * h2bS + t * h2bE;
          }, 0.0f, 1.0f, 0.5f, HitchLib.Easing.EASE_QUAD_IN_OUT);
    aniCor = null;
  }

}
