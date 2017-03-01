using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteGenerator : MonoBehaviour
{

  // Configuration:
  public GameObject notePrefab;
  public float triggerRange    = 2.0f;
  public float okayDistance    = 0.5f;
  public float perfectDistance = 0.15f;
  public float lateDistance    = 0.2f;

  // State:
  public List<NoteEntry> notes = new List<NoteEntry>();
  private static List<NoteController> liveNotes;
  private float cooldown;
  private int noteIndex;

  // Defines:
  private static readonly float[] WEIGHTED_INTERVALS = new[] { 0.5f, 0.5f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 2.0f, 2.0f };
  private static readonly float[] WEIGHTED_SUSTAINS = new[] { 0, 0, 0, 0, 0, 0, 0, 0.5f, 1.0f, 1.0f, 1.0f, 2.0f, 3.0f, 4.0f, 5.0f };

  // Messages:

  void Start()
  {
    for(int i = 0; i <= GlobalSingleton.GetScore(); ++i)
    {
      notes.Add(GenerateNote(i));
    }
    liveNotes = new List<NoteController>();
    NoteController.numMissed = 0;
  }

  void Update()
  {
    if(GlobalSingleton.GetHealth() < 0)
    {
      return;
    }

    cooldown -= Time.deltaTime * GlobalSingleton.GetBPM() * 0.0166666666f;
    if(cooldown < 0)
    {
      GameObject go = Instantiate(notePrefab);
      go.transform.SetParent(transform);
      if(noteIndex % 3 == 0)
      {
        go.transform.localPosition = new Vector3(-60.0f, -100000, 0);
      }
      else if(noteIndex % 3 == 1)
      {
        go.transform.localPosition = new Vector3(0.0f, -100000, 0);
      }
      else
      {
        go.transform.localPosition = new Vector3(60.0f, -100000, 0);
      }
      go.transform.localScale = Vector3.one;
      NoteController nc = go.GetComponent<NoteController>();
      nc.hideNote = (noteIndex - GlobalSingleton.GetHideThreshold() < 0);
      nc.id = notes[noteIndex].id;
      nc.sustainLength = (notes[noteIndex].length - 1) / 2.0f;
      nc.lateDistance = lateDistance;
      nc.lastNote = noteIndex >= (notes.Count - 1);
      if(notes[noteIndex].rest / 2.0f > 0)
      {
        while(cooldown < 0)
        {
          cooldown += notes[noteIndex].rest / 2.0f;
        }
      }
      else
      {
        cooldown = 0;
      }
      liveNotes.Add(nc);

      if(++noteIndex >= notes.Count)
      {
        cooldown -= notes[noteIndex-1].rest / 2.0f;
        noteIndex = 0;
        if(notes.Count > 8)
        {
          while(cooldown < 0)
          {
            cooldown += 4;
          }
        }
        else
        {
          while(cooldown < 0)
          {
            cooldown += 8;
          }
        }
        notes.Add(GenerateNote(notes.Count));
      }
    }
    NoteController nearestNote = null;
    float nearestDist = float.PositiveInfinity;
    NoteController nearestUnplayed = null;
    float nearestUnplayedDist = float.PositiveInfinity;
    NoteController nearestPositive = null;
    float nearestPositiveDist = float.PositiveInfinity;
    foreach(NoteController note in liveNotes)
    {
      float signedDist = note.GetDistance();
      float dist = Mathf.Abs(signedDist);

      if(dist < nearestDist && dist < triggerRange)
      {
        nearestNote = note;
        nearestDist = dist;
      }

      if(dist < nearestUnplayedDist && !note.GetHit() && dist < triggerRange)
      {
        nearestUnplayed = note;
        nearestUnplayedDist = dist;
      }

      if(signedDist >= 0 && signedDist < nearestPositiveDist && dist < triggerRange)
      {
        nearestPositive = note;
        nearestPositiveDist = signedDist;
      }
    }

    bool hitNote = false;
    int pressKey = -1;
    for(int i = 0; i < 4; ++i)
    {
      if(Input.GetKeyDown(GlobalSingleton.GetKey(i)))
      {
        pressKey = i;
        if(nearestNote != null && nearestNote.id == i &&
              (!nearestNote.GetHit() || nearestNote.GetCooldown() > 0) &&
              nearestDist < okayDistance && nearestNote.GetDistance() > -lateDistance)
        {
          hitNote = true;
          nearestNote.HitNote(perfectDistance);
        }
        else if(nearestUnplayed != null && nearestUnplayed.id == i &&
              nearestUnplayedDist < okayDistance && nearestUnplayed.GetDistance() > -lateDistance)
        {
          hitNote = true;
          nearestUnplayed.HitNote(perfectDistance);
        }
        else if(nearestPositive != null && nearestPositive.id == i &&
              (!nearestPositive.GetHit() || nearestPositive.GetCooldown() > 0) &&
              nearestPositiveDist < okayDistance && nearestPositiveDist > -lateDistance)
        {
          hitNote = true;
          nearestPositive.HitNote(perfectDistance);
        }
      }
    }
    if(pressKey >= 0 && !hitNote)
    {
      NoteController missed = null;
      if(nearestPositive != null && nearestPositive.id == pressKey)
      {
        missed = nearestPositive;
      }
      else if(nearestNote != null)
      {
        missed = nearestNote;
      }
      else if(nearestUnplayed != null)
      {
        missed = nearestUnplayed;
      }
      else if(nearestPositive != null)
      {
        missed = nearestPositive;
      }
      if(missed != null)
      {
        missed.MissNote(okayDistance);
        GlobalSingleton.GetShake().StartShake(1.0f, 0.1f);
        GlobalSingleton.GetSound().SourNote(pressKey);
      }
    }
  }

  // Utility:

  public static void RemoveNote(NoteController note)
  {
    liveNotes.Remove(note);
  }

  private NoteEntry GenerateNote(int index)
  {
    if(index < 5)
    {
      return new NoteEntry(Random.Range(0, 3),
            1,
            2);
    }
    else if(index < 10)
    {
      return new NoteEntry(Random.Range(0, 4),
            1,
            Random.Range(2, 4));
    }
    else
    {
      return new NoteEntry(Random.Range(0, 4),
            1,
            Random.Range(1, 4));
    }
  }

  private float GetInterval()
  {
    return 1.0f;
    // return WEIGHTED_INTERVALS[Random.Range(0, WEIGHTED_INTERVALS.Length)];
  }

  // NoteGenerator //*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*
  [System.Serializable]
  public struct NoteEntry
  {
    public int id;
    public int length;
    public int rest;

    public NoteEntry(int id, int length, int rest)
    {
      this.id = id;
      this.length = length;
      this.rest = rest;
    }
  }

}
