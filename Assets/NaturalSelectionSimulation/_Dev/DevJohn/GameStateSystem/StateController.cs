using System;
using UnityEngine;

namespace NaturalSelectionSimulation
{
    public class StateController : MonoBehaviour
    {
        [SerializeField]
        private bool _isPaused = false;
        [SerializeField]
        private bool _isFastForward = false;
        
        private float _iterationDuration = 1f;
        private float _advanceTimer; 



        public delegate void OnIterationAdvanceHandler();

        public static event OnIterationAdvanceHandler OnIterationAdvance;


        private void Start()
        {
            _advanceTimer = _iterationDuration;
        }

        private void Update()
        {
            if (!_isPaused)
            {
                _advanceTimer -= Time.deltaTime * (_isFastForward ? 5f : 1f);

                if (_advanceTimer <= 0) //weve made a turn
                {
                    _advanceTimer += _iterationDuration;
                    
                    OnIterationAdvance?.Invoke();
                }
            }
                
        }
    }
}