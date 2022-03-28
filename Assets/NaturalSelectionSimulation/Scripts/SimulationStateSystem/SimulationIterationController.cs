using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace NaturalSelectionSimulation
{
    public class SimulationIterationController : MonoBehaviour
    {
        #region Private Variables
        private int _iterationCount = 0;    // simple counter for currentIteration number (our 'high-score')
        private int currentScore = 0;

        public TMP_Text SimulationTimeDisplayText;
        public TMP_Text SimulationCurrentScoreDisplayText;
        public TMP_Text ScoreBonusDiplayText;

        #endregion

        private void Start()
        {
            SimulationTimeDisplayText.text = "Day 1 - 00:00";
            SimulationCurrentScoreDisplayText.text = "Current Score: 0";
            ScoreBonusDiplayText.enabled = false;
        }

        private void OnEnable()
        {
            StateController.OnIterationAdvance += AdvanceIteration; // Subscribe to AdvanceCall
        }

        private void OnDisable()
        {
            StateController.OnIterationAdvance -= AdvanceIteration; // Unsubscribe from AdvanceCall
        }

        #region Custom Methods
    
        /// <summary>
        /// Simply iterates up the count for the current iteration number
        /// </summary>
        public void AdvanceIteration()
        {
            _iterationCount++; //advance turn count
            currentScore++;

            if (_iterationCount % 1440 == 0)
            {
                currentScore += 1000;
                ShowToast(ScoreBonusDiplayText, "Day Bonus: +1000", 3);
            }
            else if (_iterationCount % 60 == 0)
            {
                currentScore += 100;
                ShowToast(ScoreBonusDiplayText, "Hour Bonus: +100", 3);
            }

            SimulationTimeDisplayText.text = $"Day {(_iterationCount / 1440) + 1} - {new TimeSpan((_iterationCount / 60) % 24, _iterationCount % 60, 0).ToString(@"hh\:mm")}";
            SimulationCurrentScoreDisplayText.text = $"Current Score: {currentScore.ToString("N0")}";
        }

        #region Toast Methods

        private void ShowToast(TMP_Text textField, string text, int duration)
        {
            StartCoroutine(showToastCOR(textField, text, duration));
        }

        private IEnumerator showToastCOR(TMP_Text textField, string text, int duration)
        {
            Color orginalColor = ScoreBonusDiplayText.color;

            textField.text = text;
            textField.enabled = true;

            //Fade in
            yield return fadeInAndOut(textField, true, 0.5f);

            //Wait for the duration
            float counter = 0;
            while (counter < duration)
            {
                counter += Time.deltaTime;
                yield return null;
            }

            //Fade out
            yield return fadeInAndOut(textField, false, 0.5f);

            textField.enabled = false;
            textField.color = orginalColor;
        }

        private IEnumerator fadeInAndOut(TMP_Text textField, bool fadeIn, float duration)
        {
            //Set Values depending on if fadeIn or fadeOut
            float a, b;
            if (fadeIn)
            {
                a = 0f;
                b = 1f;
            }
            else
            {
                a = 1f;
                b = 0f;
            }

            Color currentColor = Color.white;
            float counter = 0f;

            while (counter < duration)
            {
                counter += Time.deltaTime;
                float alpha = Mathf.Lerp(a, b, counter / duration);

                textField.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
                yield return null;
            }
        }

        #endregion

        #endregion
    }
}