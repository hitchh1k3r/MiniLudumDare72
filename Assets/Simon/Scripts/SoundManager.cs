using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

  // Configuration:
  public AudioClip[] buttonSounds = new AudioClip[4];
  public AudioClip[] playerSounds = new AudioClip[4];
  public AudioClip[] sourSounds = new AudioClip[4];
  public float volumeHit = 0.75f;
  public float volumeHold = 0.3f;
  public float volumeTwo = 0.6f;
  public float sourVolume = 0.5f;
  public float buttonMultiplier = 1.0f;
  public float playerMultiplier = 0.5f;

  // Cache:
  private AudioSource[] buttonPlayers;
  private AudioSource[] playerPlayers;
  private AudioSource oneShotPlayer;

  // State:
  private int[] numPressed;
  private Coroutine[] buttonCors;
  private Coroutine[] playerCors;

  // Messages:

  void Awake()
  {
    buttonPlayers = new AudioSource[buttonSounds.Length];
    buttonCors = new Coroutine[buttonSounds.Length];
    playerCors = new Coroutine[playerSounds.Length];
    numPressed = new int[buttonSounds.Length];

    for(int i = 0; i < buttonSounds.Length; ++i)
    {
      buttonPlayers[i] = gameObject.AddComponent<AudioSource>();
      buttonPlayers[i].mute = true;
      buttonPlayers[i].loop = true;
      buttonPlayers[i].volume = 0;
      buttonPlayers[i].clip = buttonSounds[i];
      buttonPlayers[i].Play();
    }

    playerPlayers = new AudioSource[playerSounds.Length];
    for(int i = 0; i < playerSounds.Length; ++i)
    {
      playerPlayers[i] = gameObject.AddComponent<AudioSource>();
      playerPlayers[i].mute = true;
      playerPlayers[i].loop = true;
      playerPlayers[i].volume = 0;
      playerPlayers[i].clip = playerSounds[i];
      playerPlayers[i].Play();
    }

    oneShotPlayer = gameObject.AddComponent<AudioSource>();
  }

  // Utility:

  public void PlaySound(AudioClip sound, float volume = 1)
  {
    oneShotPlayer.PlayOneShot(sound, volume);
  }

  public void SourNote(int id)
  {
    oneShotPlayer.PlayOneShot(sourSounds[id], sourVolume);
  }

  public void NoteOn(int id)
  {
    ++numPressed[id];
    if(buttonCors[id] != null)
    {
      StopCoroutine(buttonCors[id]);
    }
    if(numPressed[id] == 1)
    {
      buttonCors[id] = StartCoroutine(PlayIn(buttonPlayers[id], buttonMultiplier));
    }
    else
    {
      buttonCors[id] = StartCoroutine(PlayTo(buttonPlayers[id], volumeTwo * buttonMultiplier,
            0.1f));
    }
  }

  public void NoteOff(int id)
  {
    --numPressed[id];
    if(buttonCors[id] != null)
    {
      StopCoroutine(buttonCors[id]);
    }
    if(numPressed[id] == 0)
    {
      buttonCors[id] = StartCoroutine(PlayOut(buttonPlayers[id]));
    }
    else
    {
      buttonCors[id] = StartCoroutine(PlayTo(buttonPlayers[id], volumeHold * buttonMultiplier,
            0.25f));
    }
  }

  public void PlayerOn(int id)
  {
    if(playerCors[id] != null)
    {
      StopCoroutine(playerCors[id]);
    }
    playerCors[id] = StartCoroutine(PlayIn(playerPlayers[id], playerMultiplier));
  }

  public void PlayerOff(int id)
  {
    if(playerCors[id] != null)
    {
      StopCoroutine(playerCors[id]);
    }
    playerCors[id] = StartCoroutine(PlayOut(playerPlayers[id]));
  }

  // Corutines:

  private IEnumerator PlayIn(AudioSource player, float multiplier)
  {
    player.mute = false;
    yield return HitchLib.Tweening.GenericTween(i => { player.volume = i; }, player.volume,
          volumeHit * multiplier, 0.1f, HitchLib.Easing.EASE_QUAD_OUT);
    yield return HitchLib.Tweening.GenericTween(i => { player.volume = i; }, player.volume,
          volumeHold * multiplier, 0.1f, HitchLib.Easing.EASE_QUAD_IN_OUT);
  }

  private IEnumerator PlayOut(AudioSource player)
  {
    yield return HitchLib.Tweening.GenericTween(i => { player.volume = i; }, player.volume, 0.0f,
      0.25f, HitchLib.Easing.EASE_QUAD_IN);
    player.mute = true;
  }

  private IEnumerator PlayTo(AudioSource player, float volume, float time)
  {
    yield return HitchLib.Tweening.GenericTween(i => { player.volume = i; }, player.volume, volume,
          time, HitchLib.Easing.EASE_QUAD_IN_OUT);
  }

}
