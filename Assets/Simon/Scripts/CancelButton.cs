using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CancelButton : MonoBehaviour
{

  private bool hit;

  public void ClickButton()
  {
    if(!hit)
    {
      hit = true;
      GlobalSingleton.GetFader().Fade(() => { SceneManager.LoadScene("Reload"); });
    }
  }

}
