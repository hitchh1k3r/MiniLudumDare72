using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ScoreDisplay : MonoBehaviour
{

  // Cache:
  private Text text;

  // Messages:

  void Awake()
  {
    text = GetComponent<Text>();
  }

  void Start()
  {
    GlobalSingleton.RegisterScoreUpdateEvent(UpdateScore);
    UpdateScore();
  }

  // Event:

  void UpdateScore()
  {
    text.text = GlobalSingleton.GetScore().ToString();
  }

}
