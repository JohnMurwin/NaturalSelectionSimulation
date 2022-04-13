using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NaturalSelectionSimulation
{
    public class ScoreController : MonoBehaviour
    {
        public static void AddScore(int score, string name)
        {
            var highScore = new HighScore() { Score = score, Name = name };

            var highScoreListJson = PlayerPrefs.GetString("highscores");

            if (string.IsNullOrWhiteSpace(highScoreListJson))
            {
                var highScoreList = new HighScoreList();
                highScoreList.HighScores = new List<HighScore>() { highScore }.ToArray();

                highScoreListJson = JsonUtility.ToJson(highScoreList);

                PlayerPrefs.SetString("highscores", highScoreListJson);
                PlayerPrefs.Save();
            }
            else
            {
                var highScoreList = JsonUtility.FromJson<HighScoreList>(highScoreListJson);

                var newHighScoreList = highScoreList.HighScores.ToList();
                newHighScoreList.Add(highScore);

                highScoreList.HighScores = newHighScoreList.ToArray();
                highScoreListJson = JsonUtility.ToJson(highScoreList);

                PlayerPrefs.SetString("highscores", highScoreListJson);
                PlayerPrefs.Save();
            }
        }
    }

    [Serializable]
    public class HighScoreList
    {
        public HighScore[] HighScores;
    }

    [Serializable]
    public class HighScore
    {
        public int Score;
        public string Name;
    }
}
