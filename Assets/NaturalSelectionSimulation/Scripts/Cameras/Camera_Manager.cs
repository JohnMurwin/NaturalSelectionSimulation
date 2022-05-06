using System;
using UnityEngine;

namespace NaturalSelectionSimulation
{
    [ExecuteInEditMode]
    public class Camera_Manager : MonoBehaviour
    {
        #region Public Variables

        public bool turnFlyCamOn = false;
        public bool turnOrbitCamOn = false;
        public bool turnFollowCamOn = false;

        #endregion

        #region Private Variables

        [SerializeField] private Camera _flyCam;
        [SerializeField] private Camera _orbitCam;
        [SerializeField] private Camera _followCam;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            EnableFlyCam();
        }

        private void Update()
        {
            //! TODO: REMOVE DEBUG CODE FOR TESTING CAMERAS
            #region DEBUG
            if (turnFlyCamOn)
            {
                EnableFlyCam();
                turnFlyCamOn = false;
            }

            if (turnOrbitCamOn)
            {
                EnableOrbitCam();
                turnOrbitCamOn = false;
            }

            if (turnFollowCamOn)
            {
                EnableFollowCam();
                turnFollowCamOn = false;
            }
            #endregion
            
        }

        #endregion

        #region Custom Methods

        /// <summary>
        /// 
        /// </summary>
        public void EnableFlyCam()
        {
            _flyCam.enabled = true;
            _flyCam.GetComponent<AudioListener>().enabled = true;
            _orbitCam.enabled = false;
            _orbitCam.GetComponent<AudioListener>().enabled = false;
            _followCam.enabled = false;
            _followCam.GetComponent<AudioListener>().enabled = false;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void EnableOrbitCam()
        {
            _flyCam.enabled = false;
            _flyCam.GetComponent<AudioListener>().enabled = false;
            _orbitCam.enabled = true;
            _orbitCam.GetComponent<AudioListener>().enabled = true;
            _followCam.enabled = false;
            _followCam.GetComponent<AudioListener>().enabled = false;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void EnableFollowCam()
        {
            _flyCam.enabled = false;
            _flyCam.GetComponent<AudioListener>().enabled = false;
            _orbitCam.enabled = false;
            _orbitCam.GetComponent<AudioListener>().enabled = false;
            _followCam.enabled = true;
            _followCam.GetComponent<AudioListener>().enabled = true;
        }

        #endregion
    }
}