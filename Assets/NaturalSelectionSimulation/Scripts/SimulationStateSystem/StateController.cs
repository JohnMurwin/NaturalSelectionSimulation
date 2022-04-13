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
        
        private float _advanceTimer;
        #endregion


        #region Public Variables
        public delegate void OnIterationAdvanceHandler();
        public static event OnIterationAdvanceHandler OnIterationAdvance;

        public static event Action<bool> PauseMenuState;

        public GameObject PauseMenu = null;
        public GameObject MainCamera = null;
        public Button PauseButton = null;
        public Button PlayButton = null;
        public Button FastForwardButton = null;
        public Button SlowModeButton = null;

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
            if (!PauseMenu.activeSelf && Input.GetKeyDown(KeyCode.P))    // Pause or Play
            {
                if (_isPaused)
                    PlaySimulation();
                else
                    PauseSimulation();
            }
            
            if (!PauseMenu.activeSelf && Input.GetKeyDown(KeyCode.DownArrow))    // For Reset from Slow or Fast to Normal
                PlaySimulation();
 
            if (!PauseMenu.activeSelf && Input.GetKeyDown(KeyCode.LeftArrow))    // Slow Simulation
                SlowDownSimulation();
            
            if (!PauseMenu.activeSelf && Input.GetKeyDown(KeyCode.RightArrow))   // Fast Forward
                FastForwardSimulation();

            if (_isPaused && Input.GetKeyDown(KeyCode.M)) //temporary key binging
            {
                TogglePauseMenu();
            }

            ApplyButtonState();

            PauseMenuState?.Invoke(PauseMenu.activeSelf);

            // Core Simulation Iteration System
            if (!_isPaused)
            {
                _advanceTimer -= Time.deltaTime *  _iterationDuration;// at what speed do we advance 

                if (_advanceTimer <= 0)
                {
                    _advanceTimer += _iterationDuration;    // reset timer for next iteration
                    
                    OnIterationAdvance?.Invoke();   // invoke Advance callback
                }
            }
            
        }

        private void ApplyButtonState()
        {
            PauseButton.gameObject.SetActive(!_isPaused);
            PlayButton.gameObject.SetActive(_isPaused);

            PauseButton.enabled = 
                PlayButton.enabled = 
                FastForwardButton.enabled = 
                SlowModeButton.enabled = !PauseMenu.activeSelf;
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

            if (_iterationDuration == 0.5f)
            {
                _iterationDuration = 0.25f;
            }
            else if (_iterationDuration == 1f)
            {
                _iterationDuration = 0.5f;
            }
            if (_iterationDuration == 1.25f)
            {
                _iterationDuration = 1f;
            }
            else if (_iterationDuration == 1.5f)
            {
                _iterationDuration = 1.25f;
            }
            else if (_iterationDuration == 2f)
            {
                _iterationDuration = 1.5f;
            }
            else if (_iterationDuration == 4f)
            {
                _iterationDuration = 2f;
            }

            Time.timeScale = _iterationDuration;
            DEBUGsimulationSpeed = _iterationDuration;
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

            _iterationDuration = 1f;

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

            if (_iterationDuration == .25f)
            {
                _iterationDuration = 0.5f;
            }
            else if (_iterationDuration == 0.5f)
            {
                _iterationDuration = 1f;
            }
            if (_iterationDuration == 1f)
            {
                _iterationDuration = 1.25f;
            }
            else if (_iterationDuration == 1.25f)
            {
                _iterationDuration = 1.5f;
            }
            else if (_iterationDuration == 1.5f)
            {
                _iterationDuration = 2f;
            }
            else if (_iterationDuration == 2f)
            {
                _iterationDuration = 4f;
            }

            Time.timeScale = _iterationDuration;
            DEBUGsimulationSpeed = _iterationDuration;
        }

        public void TogglePauseMenu()
        {
            if (PauseMenu.activeSelf)
            {
                ClosePauseMenu();
            }
            else
            {
                OpenPauseMenu();
            }
        }

        public void OpenPauseMenu()
        {
            PauseSimulation();
            PauseMenu.SetActive(true);
            PauseMenuState?.Invoke(PauseMenu.activeSelf);
        }

        private void ClosePauseMenu()
        {
            PauseMenu.SetActive(false);
            PauseMenuState?.Invoke(PauseMenu.activeSelf);
        }

        #endregion

    }
}