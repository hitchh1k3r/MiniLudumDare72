using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class ComputerButton : MonoBehaviour
{

  // Config:
  public int id;
  public float offOpacity = 0.5f;
  public float onOpacity = 1.0f;

  // Cache:
  private RawImage image;
  private Color buttonColor;

  // State:
  private Coroutine aniCor;

  // Global State:
  private static ComputerButton[] instances = new ComputerButton[4];

  // Message Recivers:

  void Awake()
  {
    instances[id] = this;
    image = GetComponent<RawImage>();
  }

  void Start()
  {
    buttonColor = GlobalSingleton.GetColor(id);
    buttonColor.a = offOpacity;
    image.color = buttonColor;
  }

  // Accessor:

  public static ComputerButton GetButton(int id)
  {
    return instances[id];
  }

  // Utilities:

  private void AnimateAlpha(float i)
  {
    buttonColor.a = i;
    image.color = buttonColor;
  }

  public void ButtonDown()
  {
    if(aniCor != null)
    {
      StopCoroutine(aniCor);
    }
    aniCor = StartCoroutine(HitchLib.Tweening.GenericTween(new Action<float>(AnimateAlpha), offOpacity, onOpacity, 0.05f, HitchLib.Easing.EASE_LINEAR));
    GlobalSingleton.GetSound().NoteOn(id);
  }

  public void ButtonUp()
  {
    if(aniCor != null)
    {
      StopCoroutine(aniCor);
    }
    aniCor = StartCoroutine(HitchLib.Tweening.GenericTween(new Action<float>(AnimateAlpha), onOpacity, offOpacity, 0.25f, HitchLib.Easing.EASE_LINEAR));
    GlobalSingleton.GetSound().NoteOff(id);
  }

}
