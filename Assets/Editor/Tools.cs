using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Tools
{

  [MenuItem ("Tools/Current Aspect")]
  static void CurrentAspect()
  {
    Debug.Log(Camera.main.aspect);
  }

}
