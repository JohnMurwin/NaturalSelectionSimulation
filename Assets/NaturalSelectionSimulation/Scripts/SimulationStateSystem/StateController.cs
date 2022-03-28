using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace NaturalSelectionSimulation
{
    public class StateController : MonoBehaviour
    {
        #region Private Variables
        // TODO: convert to full StateMachine
        [SerializeField]
        private bool _isPaused = false;
        [SerializeField]
        private bool _isFastForward = false;    
        [SerializeField]
        private bool _isSlowDown = false;  

        private float _iterationDuration = 1f;
        private float _fastSpeed = 5f;
        private float _slowSpeed = 0.25f;    
        
        
        private float _advanceTimer;
        #endregion


        #region Public Variables
        public delegate void OnIterationAdvanceHandler();
        public static event OnIterationAdvanceHandler OnIterationAdvance;

        public static event Action<bool> PauseMenuState;

        public GameObject PauseMenu = null;
        public GameObject MainCamera = null;

        #endregion


        // TODO: REMOVE DEBUG VARIABLES
        public TMP_Text DEBUGsimulationSpeedText;
        public float DEBUGsimulationSpeed = 1f;
        

        private void Start()
        {
            _advanceTimer = _iterationDuration;
        }

        private void Update()
        {
            //! TODO: REMOVE after Debug
            DEBUGsimulationSpeedText.text = DEBUGsimulationSpeed.ToString() + 'x';
            
            // Input Check for System State
            if (Input.GetKeyDown(KeyCode.Space))    // Pause or Play
            {
                if (_isPaused)
                    PlaySimulation();
                else
                    PauseSimulation();
            }
            
            if (Input.GetKeyDown(KeyCode.DownArrow))    // For Reset from Slow or Fast to Normal
                PlaySimulation();
 
            if (Input.GetKeyDown(KeyCode.LeftArrow))    // Slow Simulation
                SlowDownSimulation();
            
            if (Input.GetKeyDown(KeyCode.RightArrow))   // Fast Forward
                FastForwardSimulation();

            if (_isPaused && Input.GetKeyDown(KeyCode.M)) //temporary key binging
            {
                PauseMenu.SetActive(true);
            }

            PauseMenuState?.Invoke(PauseMenu.activeSelf);

            // Core Simulation Iteration System
            if (!_isPaused)
            {
                _advanceTimer -= Time.deltaTime * (_isFastForward ? _fastSpeed : _isSlowDown ? _slowSpeed : _iterationDuration);// at what speed do we advance 

                if (_advanceTimer <= 0)
                {
                    _advanceTimer += _iterationDuration;    // reset timer for next iteration
                    
                    OnIterationAdvance?.Invoke();   // invoke Advance callback
                }
            }
            
        }

        #region Custom Methods
        /// <summary>
        /// 
        /// </summary>
        public void SlowDownSimulation()
        {
            // TODO: convert to state machine interface 
            _isPaused = false;
            _isFastForward = false;
            _isSlowDown = true;

            Time.timeScale = _slowSpeed;
            DEBUGsimulationSpeed = _slowSpeed;
        }

        /// <summary>
        /// 
        /// </summary>
        public void PauseSimulation()
        {
            // TODO: convert to state machine interface 
            _isPaused = true;
            _isFastForward = false;
            _isSlowDown = false;

            Time.timeScale = 0f;
            DEBUGsimulationSpeed = 0f;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void PlaySimulation()
        {
            // TODO: convert to state machine interface 
            _isPaused = false;
            _isFastForward = false;
            _isSlowDown = false;

            Time.timeScale = _iterationDuration;
            DEBUGsimulationSpeed = _iterationDuration;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void FastForwardSimulation()
        {
            // TODO: convert to state machine interface
            _isPaused = false;
            _isFastForward = true;
            _isSlowDown = false;
            
            Time.timeScale = _fastSpeed;
            DEBUGsimulationSpeed = _fastSpeed;
        }

        public void OpenPauseMenu()
        {
            PauseSimulation();
            PauseMenu.SetActive(true);
            PauseMenuState?.Invoke(PauseMenu.activeSelf);
        }

        #endregion

    }
}