using UnityEngine;

namespace NaturalSelectionSimulation
{
    public class Camera_Follow : MonoBehaviour
    {
        #region Public Variables

        

        #endregion

        #region Private Variables
        
        [SerializeField][Tooltip("Object to follow & rotate around")]
        private GameObject _target;
        [SerializeField][Tooltip("Distance to orbit around")]
        private float _distance = 10.0f;
        [SerializeField][Tooltip("X-axis rotate speed")]
        private float _xSpeed = 250.0f;
        [SerializeField][Tooltip("Y-axis rotate speed")]
        private float _ySpeed = 120.0f;
        [SerializeField][Tooltip("Y-axis lower angle limit")]
        private float _yMinLimit = -20;
        [SerializeField][Tooltip("Y-axis upper angle limit")]
        private float _yMaxLimit = 80;
        
        private float x = 0.0f;
        private float y = 0.0f;
        private float prevDistance;

        #endregion

        #region Unity Methods
        
        void Start()
        {
            var angles = transform.eulerAngles;
            x = angles.y;
            y = angles.x;
        }

        void LateUpdate()
        {
            if (_distance < 2) _distance = 2;
                _distance -= Input.GetAxis("Mouse ScrollWheel") * 2;
            if (_target)
            {
                var pos = Input.mousePosition;
                var dpiScale = 1f;
                if (Screen.dpi < 1) dpiScale = 1;
                if (Screen.dpi < 200) dpiScale = 1;
                else dpiScale = Screen.dpi / 200f;

                if (pos.x < 380 * dpiScale && Screen.height - pos.y < 250 * dpiScale) return;

                // comment out these two lines if you don't want to hide mouse curser or you have a UI button 
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

                x += Input.GetAxis("Mouse X") * _xSpeed * 0.02f;
                y -= Input.GetAxis("Mouse Y") * _ySpeed * 0.02f;

                y = ClampAngle(y, _yMinLimit, _yMaxLimit);
                var rotation = Quaternion.Euler(y, x, 0);
                transform.rotation = rotation;
                
                var position = rotation * new Vector3(0.0f, 0.0f, -_distance) + _target.transform.position;
                transform.position = position;

            }
            else
            {
                // comment out these two lines if you don't want to hide mouse curser or you have a UI button 
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }

            if (Mathf.Abs(prevDistance - _distance) > 0.001f)
            {
                prevDistance = _distance;
                var rot = Quaternion.Euler(y, x, 0);
                var po = rot * new Vector3(0.0f, 0.0f, -_distance) + _target.transform.position;
                transform.rotation = rot;
                transform.position = po;
            }
        }

        #endregion

        #region Custom Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360)
                angle += 360;
            if (angle > 360)
                angle -= 360;
            return Mathf.Clamp(angle, min, max);
        }
        
        #endregion
    }
}