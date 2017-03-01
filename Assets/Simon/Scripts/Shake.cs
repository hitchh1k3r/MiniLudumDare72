using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{

  // Configuration:
  public float amount = 10.0f;

  // Cache:
  private Vector3 basePosition;

  // State:
  private float duration;
  private float intensity;
  private float startPower;

  // Messages:

  void Awake()
  {
    basePosition = transform.localPosition;
  }

  void Update()
  {
    Vector3 offset = Vector3.zero;
    if(intensity > 0)
    {
      float a = Random.Range(0.0f, 2.0f * Mathf.PI);
      Vector3 dir = new Vector3(Mathf.Cos(a), Mathf.Sin(a), 0);
      offset = (intensity * amount) * dir;

      intensity -= startPower * (Time.deltaTime / duration);
    }
    transform.localPosition = basePosition + offset;
  }

  // Utilities:

  public void StartShake(float power, float duration)
  {
    if(power > intensity)
    {
      startPower = power;
      intensity = power;
      this.duration = duration;
    }
    else if(duration > this.duration)
    {
      this.duration = duration;
    }
  }

}
