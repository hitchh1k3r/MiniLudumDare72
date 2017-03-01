using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class VersionNumber : MonoBehaviour
{

  // Configuration:
  public TextAsset versionText;

  void Start()
  {
    GetComponent<Text>().text = versionText.text;
  }

}
