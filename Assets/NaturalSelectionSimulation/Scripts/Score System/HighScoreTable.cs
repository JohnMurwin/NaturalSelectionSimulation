using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace NaturalSelectionSimulation
{
    public class HighScoreTable : MonoBehaviour
    {
        public GameObject highscoreContainer;
        public GameObject highscoreRow;

        private List<HighScore> highScores;

        private void Awake()
        {
            highscoreRow.SetActive(false);

            string highScoresJson = PlayerPrefs.GetString("highscores");
            if (!string.IsNullOrWhiteSpace(highScoresJson))
            {
                var highScoreList = JsonUtility.FromJson<HighScoreList>(highScoresJson);

                highScores = highScoreList.HighScores.ToList();
                highScores = highScores.OrderByDescending(s => s.Score).Take(10).ToList();

                float rowHeight = 50f;
                float rowYPosition = 249f;

                for (int i = 0; i < highScores.Count; i++)
                {
                    Transform row = Instantiate(highscoreRow.transform, highscoreContainer.transform);
                    var rowRectTransform = row.GetComponent<RectTransform>();
                    rowRectTransform.anchoredPosition = new Vector2(0, (-rowHeight * i) + rowYPosition);

                    row.Find("posText").GetComponent<TMP_Text>().text = (i + 1).ToString();
                    row.Find("scoreText").GetComponent<TMP_Text>().text = highScores[i].Score.ToString("N0");
                    row.Find("nameText").GetComponent<TMP_Text>().text = highScores[i].Name;

                    row.gameObject.SetActive(true);
                }
            }
        }
    }
}