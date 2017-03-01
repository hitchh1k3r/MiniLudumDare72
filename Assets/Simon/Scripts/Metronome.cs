using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metronome : MonoBehaviour
{

  // Configuration:
  public AudioClip click;

  // State:
  private float cooldown = 1;

  // Messages:

  void Update()
  {
    cooldown -= Time.deltaTime * GlobalSingleton.GetBPM() * 0.0166666666f;
    if(cooldown < 0)
    {
      while(cooldown < 0)
      {
        cooldown += 1;
      }
      GlobalSingleton.GetSound().PlaySound(click);
    }
  }

}
