using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SubmitButton : MonoBehaviour
{

  public UsernameCache username;

  private bool hit;

  public void ClickButton()
  {
    if(!hit)
    {
      hit = true;
      StartCoroutine(SubmitScore(username.SubmitName(), GlobalSingleton.GetScore()));
    }
  }

  // Corutines:

  private IEnumerator SubmitScore(string username, int score)
  {
    if(username == null || username.Trim() == "")
    {
      username = "Anonymous";
    }

    WWW request = new WWW("https://dollarone.games/elympics/submitHighscore?" +
          "key=[REDACTED]&name=" + WWW.EscapeURL(username) + "&score=" + score);
    yield return request;

    GlobalSingleton.GetFader().Fade(() => { SceneManager.LoadScene("Reload"); });
  }

}
