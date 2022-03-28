using System;
using TMPro;
using UnityEngine;

namespace NaturalSelectionSimulation
{
    public class SimulationIterationController : MonoBehaviour
    {
        #region Private Variables
        private int _iterationCount = 0;    // simple counter for currentIteration number (our 'high-score')

        public TMP_Text SimulationTimeDisplayText;

        #endregion

        private void Start()
        {
            SimulationTimeDisplayText.text = "Day 1 - 00:00";
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
            SimulationTimeDisplayText.text = $"Day {(_iterationCount / 1440) + 1} - {new TimeSpan((_iterationCount / 60) % 24, _iterationCount % 60, 0).ToString(@"hh\:mm")}";
        }
        #endregion
    }
}