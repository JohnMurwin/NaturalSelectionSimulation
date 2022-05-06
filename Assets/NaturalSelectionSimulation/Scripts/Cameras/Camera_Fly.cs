using System;
using UnityEngine;

namespace NaturalSelectionSimulation
{
    public class Camera_Fly : MonoBehaviour
    {
	    #region Public Variables


	    #endregion

	    #region Private Variables
	    
		// config variables for camera attributes
	    [SerializeField][Tooltip("How fast the camera moves")]
	    private float _acceleration = 30;
	    [SerializeField][Tooltip("How quickly the camera looks around")]
	    private float _lookSensitivity = 5;
	    [SerializeField][Tooltip("How quickly you stop movement after no input")]
	    private float _dampingCoefficient = 5;
	    [SerializeField][Tooltip("Camera minimum Field Of View (for zoom)")]
	    private float _minFOV = 15f;
	    [SerializeField][Tooltip("Camera minimum Field Of View (for zoom)")]
	    private float _maxFOV = 90f;
	    [SerializeField][Tooltip("How fast zoom is")]
	    private float _zoomIncriment = 2f;

	    // keys for control & movement
	    private KeyCode _forward = KeyCode.W;
	    private KeyCode _backward = KeyCode.S;
	    private KeyCode _up = KeyCode.E;
	    private KeyCode _down = KeyCode.Q;
	    private KeyCode _left = KeyCode.A;
	    private KeyCode _right = KeyCode.D;
	    private KeyCode _focus = KeyCode.F;

	    private Vector3 velocity; // current velocity
	    private Camera _camera;	// camera storage for cam operations

	    #endregion

	    #region Unity Methods
	    
        private void Start()
        {
        	_camera = Camera.main;
            _lookSensitivity = PlayerPrefs.GetFloat("masterMouseSensitivity", 1);
        }

        private void Update()
        {
	        // Locks Cursor Movement
	        Cursor.lockState = CursorLockMode.Locked;
	        Cursor.visible = false;
        }

        void FixedUpdate()
        {
	        UpdateInput();	// get new movement input every frame

	        // camera physics and translation
        	velocity = Vector3.Lerp(velocity, Vector3.zero, _dampingCoefficient * Time.deltaTime);
        	transform.position += velocity * Time.deltaTime;
    
    
        	// this controls the zoom using the scroll wheel
            float fov = _camera.fieldOfView;
            fov -= Input.GetAxis("Mouse ScrollWheel") * _zoomIncriment;
            fov = Mathf.Clamp(fov, _minFOV, _maxFOV);
            _camera.fieldOfView = fov;
	            

            // makes sure the camera can't go below the ground
        	if (transform.position.y <= 2)
	            transform.position = new Vector3(transform.position.x, 2f, transform.position.z);


            // the part that deals with rotating the camera around using the right click
        	if (Input.GetMouseButton(1))
        	{
        		//transform.RotateAround(target.transform.position, transform.up, Input.GetAxis("Mouse X") * _speed);
        		//transform.RotateAround(target.transform.position, transform.right, Input.GetAxis("Mouse Y") * -_speed);
        	}
        }

        #endregion

        #region Custom Methods
        
        /// <summary>
        /// 
        /// </summary>
        void UpdateInput()
        {
	        // Position
	        velocity += GetAccelerationVector() * Time.deltaTime;
	        Vector2 mouseDelta;

	        // Rotation
	        if (PlayerPrefs.GetInt("masterInvertY", -1) == 1)
		        mouseDelta = _lookSensitivity * new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
		    else
		        mouseDelta = _lookSensitivity * new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));

	        Quaternion rotation = transform.rotation;
	        Quaternion horiz = Quaternion.AngleAxis(mouseDelta.x, Vector3.up);
	        Quaternion vert = Quaternion.AngleAxis(mouseDelta.y, Vector3.right);
	        transform.rotation = horiz * rotation * vert;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Vector3 GetAccelerationVector()
        {
	        Vector3 moveInput = default;

	        void AddMovement(KeyCode key, Vector3 dir)
	        {
		        if (Input.GetKey(key))
			        moveInput += dir;
	        }

	        AddMovement(_forward, Vector3.forward);
	        AddMovement(_backward, Vector3.back);
	        AddMovement(_right, Vector3.right);
	        AddMovement(_left, Vector3.left);
	        AddMovement(_up, Vector3.up);
	        AddMovement(_down, Vector3.down);
	        
	        Vector3 direction = transform.TransformVector(moveInput.normalized);

	        return direction * _acceleration;
        }

        #endregion
    }
}