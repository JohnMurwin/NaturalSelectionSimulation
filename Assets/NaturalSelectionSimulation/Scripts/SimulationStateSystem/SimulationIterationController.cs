using System;
using UnityEngine;

namespace NaturalSelectionSimulation
{
    public class SimulationIterationController : MonoBehaviour
    {
        #region Private Variables
        private int _iterationCount = 0;    // simple counter for currentIteration number (our 'high-score')

        #endregion
        
        
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
        }
        #endregion
    }
}