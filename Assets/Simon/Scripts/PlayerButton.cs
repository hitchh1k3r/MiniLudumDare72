using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class PlayerButton : MonoBehaviour
{

  // Config:
  public int id;
  public Text keyText;
  public float offOpacity = 0.5f;
  public float onOpacity = 1.0f;

  // Cache:
  private RawImage image;
  private Color buttonColor;
  private KeyCode keyCode;

  // State:
  private Coroutine aniCor;
  private bool pressed;

  // Message Recivers:

  void Awake()
  {
    image = GetComponent<RawImage>();
  }

  void Start()
  {
    buttonColor = GlobalSingleton.GetColor(id);
    buttonColor.a = offOpacity;
    image.color = buttonColor;
    KeyRemap();
  }

  void KeyRemap()
  {
    keyCode = GlobalSingleton.GetKey(id);
    keyText.text = GlobalSingleton.GetKeyName(id).ToString();
  }

  void Update()
  {
    if(GlobalSingleton.GetHealth() < 0)
    {
      if(pressed)
      {
        GlobalSingleton.GetSound().PlayerOff(id);
        pressed = false;
      }
      return;
    }

    if(Input.GetKeyDown(keyCode))
    {
      if(aniCor != null)
      {
        StopCoroutine(aniCor);
      }
      aniCor = StartCoroutine(HitchLib.Tweening.GenericTween(new Action<float>(AnimateAlpha), offOpacity, onOpacity, 0.05f, HitchLib.Easing.EASE_LINEAR));
      GlobalSingleton.GetSound().PlayerOn(id);
      pressed = true;
    }
    if(Input.GetKeyUp(keyCode))
    {
      if(aniCor != null)
      {
        StopCoroutine(aniCor);
      }
      aniCor = StartCoroutine(HitchLib.Tweening.GenericTween(new Action<float>(AnimateAlpha), onOpacity, offOpacity, 0.25f, HitchLib.Easing.EASE_LINEAR));
      GlobalSingleton.GetSound().PlayerOff(id);
      pressed = false;
    }
  }

  // Utility:

  void AnimateAlpha(float i)
  {
    buttonColor.a = i;
    image.color = buttonColor;
  }

}
