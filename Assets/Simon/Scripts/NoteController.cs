using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class NoteController : MonoBehaviour
{

  // Configure:
  public int id;
  public RawImage border;
  public RawImage[] noteColors;
  public bool hideNote;
  public float sustainLength;
  public float lateDistance;
  public RectTransform tail;
  public Text keyText;
  public Color missBorder;
  public Color hitBorder;
  public bool lastNote;
  public GameObject wrongNote, tooEarly, tooLate, perfect, okay;

  // Cahce:
  private RectTransform rTrans;

  // State:
  public static int numMissed;

  private float beat = -7.5f;
  private bool computerHit;
  private bool hideHit;
  private bool playerMissed;
  private bool playerHit;
  private float hitCooldown;

  // Defines:
  public const float BEAT_SIZE = 80;
  public const float BEAT_PLAYER_Y = -265.0f;
  private const float COMPUTER_BEAT = -7;
  private const float HIDDING_BEAT = -3.5f;
  private const float DESTROY_BEAT = 1.0f;

  // Messages:

  void Awake()
  {
    rTrans = GetComponent<RectTransform>();
  }

  void Start()
  {
    Color c = GlobalSingleton.GetColor(id);
    for(int i = 0; i < noteColors.Length; ++i)
    {
      noteColors[i].color = c;
    }
    if(sustainLength > 0)
    {
      tail.gameObject.SetActive(true);
      tail.sizeDelta = new Vector2(12, sustainLength * BEAT_SIZE);
      tail.gameObject.AddComponent<MatchScale>();
    }
    KeyRemap();
  }

  void KeyRemap()
  {
    keyText.text = GlobalSingleton.GetKeyName(id).ToString();
  }

  void Update()
  {
    if(GlobalSingleton.GetHealth() < 0)
    {
      return;
    }

    if(hitCooldown > 0)
    {
      hitCooldown -= Time.deltaTime;
    }
    beat += GlobalSingleton.GetBPM() * 0.0166666666666f * Time.deltaTime;

    if(beat > DESTROY_BEAT + sustainLength)
    {
      NoteGenerator.RemoveNote(this);
      Destroy(gameObject);
      return;
    }

    if(beat > lateDistance && !playerMissed && !playerHit)
    {
      GlobalSingleton.GetShake().StartShake(0.5f, 0.1f);
      MissNote(0);
    }

    if(beat > lateDistance && lastNote)
    {
      lastNote = false;
      if(numMissed <= 0)
      {
        GlobalSingleton.IncreaseScore();
      }
      numMissed = 0;
    }

    if(beat >= COMPUTER_BEAT && !computerHit)
    {
      computerHit = true;
      StartCoroutine(PlayNote());
    }

    if(beat >= HIDDING_BEAT && !hideHit && hideNote)
    {
      hideHit = true;
      for(int i = 0; i < noteColors.Length; ++i)
      {
        StartCoroutine(HitchLib.Tweening.FadeTo(noteColors[i], 60f / GlobalSingleton.GetBPM(),
              HitchLib.Easing.EASE_LINEAR, 0));
      }
      StartCoroutine(HitchLib.Tweening.FadeTo(keyText, 60f / GlobalSingleton.GetBPM(),
            HitchLib.Easing.EASE_LINEAR, 0));
    }

    float yPos = BEAT_PLAYER_Y - (beat * BEAT_SIZE);
    Vector3 pos = rTrans.localPosition;
    pos.y = yPos * GlobalSingleton.GetTimelineScale();
    rTrans.localPosition = pos;
  }

  // Accessors:

  public bool GetHit()
  {
    return playerHit;
  }

  public bool GetMiss()
  {
    return playerMissed;
  }

  public float GetCooldown()
  {
    return hitCooldown;
  }

  // Utilities:

  public void MissNote(float okayDistance)
  {
    if(!playerMissed && !playerHit)
    {
      ++numMissed;
      StartCoroutine(HitchLib.Tweening.GenericTween(c => { border.color = c; }, border.color,
            missBorder, 0.1f, HitchLib.Easing.EASE_LINEAR));
    }
    playerMissed = true;
    GlobalSingleton.Hurt();
    if(Mathf.Abs(beat) < okayDistance)
    {
      Popup(wrongNote);
    }
    else if(beat < 0)
    {
      Popup(tooEarly);
    }
    else
    {
      Popup(tooLate);
    }
  }

  public void HitNote(float perfectDistance)
  {
    playerHit = true;
    if(hitCooldown > 0)
    {
      return;
    }

    hitCooldown = 0.1f;
    if(playerMissed)
    {
      --numMissed;
    }
    StartCoroutine(HitchLib.Tweening.GenericTween(c => { border.color = c; }, border.color,
          hitBorder, 0.1f, HitchLib.Easing.EASE_LINEAR));
    if(Mathf.Abs(beat) < perfectDistance)
    {
      Popup(perfect);
    }
    else
    {
      Popup(okay);
    }
  }

  public float GetDistance()
  {
    return -beat;
  }

  private void Popup(GameObject prefab)
  {
    GameObject go = Instantiate(prefab);
    go.transform.SetParent(transform.parent);
    go.transform.position = transform.position;
  }

  // Corutines:

  private IEnumerator PlayNote()
  {
    ComputerButton.GetButton(id).ButtonDown();
    GlobalSingleton.GetSound().NoteOn(id);
    yield return new WaitForSeconds(((sustainLength + 0.25f) * 60f) / GlobalSingleton.GetBPM());
    GlobalSingleton.GetSound().NoteOff(id);
    ComputerButton.GetButton(id).ButtonUp();
  }

}
