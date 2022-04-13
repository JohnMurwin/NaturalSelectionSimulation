using System;
using UnityEngine;

namespace NaturalSelectionSimulation
{
    public class OrbitCamera_Manager : MonoBehaviour
    {
        private float _moveAngle = 30f;
        private float cameraMoveSpeed = .15f;
        
        public Transform lookTarget;

        private void Start()
        {
            transform.LookAt(lookTarget);
        }

        private void Update()
        {
            transform.RotateAround(lookTarget.transform.position, Vector3.up,  _moveAngle * Time.unscaledDeltaTime * cameraMoveSpeed);
        }
    }
}