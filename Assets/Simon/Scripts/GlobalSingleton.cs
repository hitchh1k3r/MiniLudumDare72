using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalSingleton : MonoBehaviour
{

#if UNITY_EDITOR
  // Config Special:
  public int editorStartLevel = 0;
#endif

  // Configuration:
  public int health = 3;
  public int score;
  public int hideThreshold = 10;
  public float bpm = 60;
  public RectTransform timelineScale;
  public Color[] pallet = new Color[4];
  public KeyCode[] keys = new KeyCode[4];
  public char[] symbols = new char[4];
  public SoundManager soundManager;
  public Shake screenShake;
  public SceneFader screenFader;
  public CanvasGroup inputGroup;

  // Events:
  public delegate void Trigger();
  public event Trigger ScoreUpdate;

  // Instance:
  private static GlobalSingleton instance;

  // Messages:

  void Awake()
  {
    instance = this;
  }

#if UNITY_EDITOR
  void Start()
  {
    instance.score = editorStartLevel - 1;
    IncreaseScore();
  }
#endif

  // Accessors:

#if UNITY_EDITOR
  public static int GetEditorStart()
  {
    return instance.editorStartLevel;
  }
#endif

  public static void RegisterScoreUpdateEvent(Trigger del)
  {
    instance.ScoreUpdate += del;
  }

  public static void IncreaseScore()
  {
    ++instance.score;
    if(instance.score > 5)
    {
      instance.bpm += 1;
    }
    if(instance.score > 10)
    {
      instance.bpm += 1;
    }
    if(instance.score > 20)
    {
      instance.bpm += 3;
    }
    if(instance.score > 30)
    {
      instance.bpm += 5;
    }
    if(instance.bpm > 160)
    {
      instance.bpm = 160;
    }
    instance.hideThreshold = (instance.score + 1) / 5;
    instance.ScoreUpdate();
    GlobalSingleton.Heal();
  }

  public static int GetScore()
  {
    return instance.score;
  }

  public static int GetHealth()
  {
    return instance.health;
  }

  public static int GetHideThreshold()
  {
    return instance.hideThreshold;
  }

  public static float GetBPM()
  {
    return instance.bpm;
  }

  public static float GetTimelineScale()
  {
    return instance.timelineScale.localScale.y;
  }

  public static Color GetColor(int id)
  {
    return instance.pallet[id];
  }

  public static KeyCode GetKey(int id)
  {
    return instance.keys[id];
  }

  public static char GetKeyName(int id)
  {
    return instance.symbols[id];
  }

  public static SoundManager GetSound()
  {
    return instance.soundManager;
  }

  public static SceneFader GetFader()
  {
    return instance.screenFader;
  }

  public static Shake GetShake()
  {
    return instance.screenShake;
  }

  // Utilities:

  public static void Heal()
  {
    ++instance.health;
    if(instance.health > 3)
    {
      instance.health = 3;
    }
  }

  public static void Hurt()
  {
    --instance.health;
    if(instance.health < 0)
    {
      if(instance.score <= 0)
      {
        instance.screenFader.Fade(() => { SceneManager.LoadScene("Reload"); });
      }
      else
      {
        instance.inputGroup.interactable = true;
        instance.StartCoroutine(HitchLib.Tweening.GenericTween(
              t => { instance.inputGroup.alpha = t; }, 0, 1, 0.5f, HitchLib.Easing.EASE_QUAD_IN));
      }
    }
  }

}
