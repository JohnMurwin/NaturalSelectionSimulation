using System;
using UnityEngine;

namespace NaturalSelectionSimulation
{
    public class SimulationIterationController : MonoBehaviour
    {
        private int _iterationCount = 0;
        
        private void OnEnable()
        {
            StateController.OnIterationAdvance += AdvanceIteration;
        }

        private void OnDisable()
        {
            StateController.OnIterationAdvance -= AdvanceIteration;
        }

        public void AdvanceIteration()
        {
            _iterationCount++; //advance turn count
            
            Debug.Log("Its iteration: " + _iterationCount);
            
        }
    }
}