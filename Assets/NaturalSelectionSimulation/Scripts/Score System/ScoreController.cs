using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

namespace NaturalSelectionSimulation
{
    public class ScoreController : MonoBehaviour
    {
        public TMP_Text rabbitCountText;
        public TMP_Text wolfCountText;
        
        private int rabbitCount = 0;

        private GameObject[] rabbits;

        private StateController _stateController;

        private void Start()
        {
            _stateController = GameObject.Find("SimlationStateSystem").GetComponent<StateController>();
        }

        private void Update()
        {
            rabbitCount = 0;
            
            rabbits = GameObject.FindGameObjectsWithTag("Rabbit");

            foreach (GameObject rabbit in rabbits)
            {
                rabbitCount++;
            }
            
            rabbitCountText.text = "Rabbit Count: " + rabbitCount.ToString();

            if (rabbitCount == 0)
            {
                Debug.Log("We should end the game now....");
                _stateController.EndGameSimulation();
            }
        }

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
