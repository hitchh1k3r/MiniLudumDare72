using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class UsernameCache : MonoBehaviour
{

  // Configuration:
  public string[] names = new[]{
    "Anonymous Alliteration",
    "Anonymous Aardvark",
    "Anonymous Anaconda",
    "Anonymous Alligator",
    "Nameless Narwhal",
    "Nameless Nautilus",
    "Nameless Newt",
    "Nameless Needlefish",
    "Secret Starfish",
    "Secret Salmon",
    "Secret Swan",
    "Secret Squirrel"
  };

  // Cache:
  private InputField input;

  // State:
  public static string username;

  // Messages:

  void Start()
  {
    input = GetComponent<InputField>();

    if(username != null)
    {
      input.text = username;
    }
    else
    {
      input.text = names[Random.Range(0, names.Length)];
    }
  }

  // Utilities:

  public string SubmitName()
  {
    username = input.text;
    return username;
  }

}
