using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scores : MonoBehaviour
{

  // Configuration:
  public GameObject scoreEntry;

  // Defines:
  private ScoreEntry[] scores = new[]{
    new ScoreEntry(1, "Able Albatross", 20),
    new ScoreEntry(2, "Experienced Eel", 18),
    new ScoreEntry(2, "Adept Anteater", 18),
    new ScoreEntry(4, "Proficient Penguin", 16),
    new ScoreEntry(5, "Expert Elephant", 15),
    new ScoreEntry(6, "Capable Coyote", 11),
    new ScoreEntry(6, "Decent Dingo", 11),
    new ScoreEntry(8, "Sufficient Sea Gull", 10),
    new ScoreEntry(9, "Acceptable Armadillo", 9),
    new ScoreEntry(10, "Fair Fox", 8),
    new ScoreEntry(11, "Clumsy Cricket", 3),
    new ScoreEntry(12, "Inept Iguana", 2),
    new ScoreEntry(12, "Inproficient Insect", 2),
    new ScoreEntry(14, "Awkward Alpaca", 1),
    new ScoreEntry(14, "Unqualified Urchin", 1),
  };

  // Messages:

  IEnumerator Start()
  {
    WWW request = new WWW("https://dollarone.games/elympics/getHighscores?username=hitchh1k3r&unique=true");
    yield return request;
    if(string.IsNullOrEmpty(request.error))
    {
      List<ScoreEntry> newScores = new List<ScoreEntry>();
      HighScoreData data = JsonUtility.FromJson<HighScoreData>("{\"scores\":" + request.text + "}");
      int count = 0;
      for(int i = 0; i < data.scores.Length; ++i)
      {
        if(data.scores[i].score > 0 && data.scores[i].name != null &&
              data.scores[i].name.Trim() != "")
        {
          if(++count >= 15)
          {
            newScores.Add(new ScoreEntry(0, data.scores[i].name, data.scores[i].score));
          }
        }
      }
      newScores.Sort((x, y) => (y.score - x.score));
      scores = newScores.ToArray();
      int rank = 1;
      int runRank = 1;
      int lastScore = int.MaxValue;
      for(int i = 0; i < scores.Length; ++i)
      {
        if(lastScore > scores[i].score)
        {
          rank = runRank;
          lastScore = scores[i].score;
        }
        scores[i].rank = rank;
        ++runRank;
      }
    }

    for(int i = 0; i < 25 && i < scores.Length; ++i)
    {
      GameObject newScore = Instantiate(scoreEntry);
      ScoreEntryLine line = newScore.GetComponent<ScoreEntryLine>();
      scores[i].line = line;
      newScore.transform.SetParent(transform);
      RectTransform rTrans = (RectTransform) newScore.transform;
      rTrans.localScale = Vector3.one;
      rTrans.offsetMin = new Vector2( 10, -45 - (i * 35));
      rTrans.offsetMax = new Vector2(-20, -15 - (i * 35));
      DrawScore(scores[i]);
    }

    yield break;
  }

  // Utilities:

  private void DrawScore(ScoreEntry score)
  {
    score.line.rank.text = score.rank.ToString();
    score.line.name.text = score.name;
    score.line.score.text = score.score.ToString();
  }

  // Scores //*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*/
  [System.Serializable]
  public struct HighScoreData
  {
    public HighScoreEntry[] scores;
  }

  // Scores //*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*/
  [System.Serializable]
  public struct HighScoreEntry
  {
    public string name;
    public int score;
  }

  // Scores //*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*/
  [System.Serializable]
  public struct ScoreEntry
  {
    public int rank;
    public string name;
    public int score;
    public ScoreEntryLine line;

    public ScoreEntry(int rank, string name, int score)
    {
      this.rank = rank;
      this.name = name;
      this.score = score;
      line = null;
    }
  }

}
