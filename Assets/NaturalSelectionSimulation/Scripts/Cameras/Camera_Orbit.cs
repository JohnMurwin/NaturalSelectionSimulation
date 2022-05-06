using System;
using UnityEngine;

namespace NaturalSelectionSimulation
{
    public class Camera_Orbit : MonoBehaviour
    {
        #region Private Variables
        
        [SerializeField][Tooltip("How fast the camera will rotate around target")]
        private float _speed = 10f;
        [SerializeField][Tooltip("What object the camera will rotate around. (Center 'LookTarget' in the map.")]
        private Transform _target;

        #endregion

        #region Unity Methods
        
        private void Update()
        {
            // Locks Cursor Movement
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void FixedUpdate()  //? using FixedUpdate instead of Update/LateUpdate to ensure physics step is even for smooth camera rotate
        {
            transform.LookAt(_target);
            transform.Translate(Vector3.right * Time.deltaTime * _speed);
        }
        #endregion
    }
}