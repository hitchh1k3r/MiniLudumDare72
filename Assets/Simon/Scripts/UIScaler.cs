using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScaler : MonoBehaviour
{

  public RectTransform matchHeight;

  private float referanceHeight;

  void OnEnable()
  {
    referanceHeight = ((RectTransform) transform).rect.height;
  }

  void Update()
  {
    transform.localScale = new Vector3(1, matchHeight.rect.height / referanceHeight, 1);
  }

}
