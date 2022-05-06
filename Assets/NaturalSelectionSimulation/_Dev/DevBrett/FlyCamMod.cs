using UnityEngine;
using System.Collections;
using NaturalSelectionSimulation;

[RequireComponent(typeof(Camera))]
public class FlyCamMod : MonoBehaviour
{
	private float _acceleration = 30; // how fast the camera moves
	private float lookSensitivity = 1; // mouse look sensitivity
	public float dampingCoefficient = 5; // how quickly you break to a halt after you stop your input
	private bool _focusOnEnable = true; // whether or not to focus when camera starts up

	public static FlyCamMod instance;
	public Transform followTransform;

	Camera _camera;
	public Camera orbitCamera;

	// variables setting the keys for movement
	private KeyCode _forward = KeyCode.W;
	private KeyCode _backward = KeyCode.S;
	private KeyCode _up = KeyCode.Q;
	private KeyCode _down = KeyCode.E;
	private KeyCode _left = KeyCode.A;
	private KeyCode _right = KeyCode.D;
	private KeyCode _focus = KeyCode.F;

	Vector3 velocity; // current velocity

	private float _zoomIncriment = 20f; // how fast the zoom is
	static private bool _focused;

	private float _speed = 5;
	private bool _isPaused;

	void OnEnable()
	{
		if (_focusOnEnable) _focused = true;
		StateController.PauseMenuState += StateController_PauseMenuState;
	}

	void OnDisable()
	{
		_focused = false;
		StateController.PauseMenuState -= StateController_PauseMenuState;
	}

	private void Start()
	{
		_camera = Camera.main;
		_camera.enabled = true;
		orbitCamera.enabled = false;
		instance = this;
		lookSensitivity = PlayerPrefs.GetFloat("masterMouseSensitivity", 1);
	}

	void Update()
	{

		if (followTransform != null)
		{
			transform.position = followTransform.position;
		}

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			followTransform = null;
		}

		if (_isPaused || Input.GetKeyDown(KeyCode.Space))
		{
			if (_camera.enabled)
			{
				orbitCamera.enabled = true;
				_camera.enabled = false;
			}
			else if (!_camera.enabled)
			{
				orbitCamera.enabled = false;
				_camera.enabled = true;
			}
		}

		// Input
		if (_focused)
		{
			UpdateInput();
		}
		else if (Input.GetKeyDown(_focus))
		{
			_focused = true;
		}

		// Physics of the camera
		velocity = Vector3.Lerp(velocity, Vector3.zero, dampingCoefficient * Time.deltaTime);
		transform.position += velocity * Time.deltaTime;


		// this controls the zoom using the scroll wheel
		if (Camera.main.orthographic)
		{
			Camera.main.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * _zoomIncriment;
		}
		else
		{
			Camera.main.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * _zoomIncriment;
		}

		// makes sure the camera can't go below the ground
		if (transform.position.y <= 2)
		{
			transform.position = new Vector3(transform.position.x, 2f, transform.position.z);
		}


		// the part that deals with rotating the camera around using the right click
		if (Input.GetMouseButton(1))
		{
			//transform.RotateAround(target.transform.position, transform.up, Input.GetAxis("Mouse X") * _speed);
			//transform.RotateAround(target.transform.position, transform.right, Input.GetAxis("Mouse Y") * -_speed);
		}
	}

	void UpdateInput()
	{
		// Position
		velocity += GetAccelerationVector() * Time.deltaTime;
		Vector2 mouseDelta;

		// Rotation
		if (PlayerPrefs.GetInt("masterInvertY", -1) == 1)
		{
			mouseDelta = lookSensitivity * new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
		}
		else
		{
			mouseDelta = lookSensitivity * new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
		}

		Quaternion rotation = transform.rotation;
		Quaternion horiz = Quaternion.AngleAxis(mouseDelta.x, Vector3.up);
		Quaternion vert = Quaternion.AngleAxis(mouseDelta.y, Vector3.right);
		transform.rotation = horiz * rotation * vert;

		// unlock the camera for additional movement
		if (Input.GetKeyDown(KeyCode.Escape))
			_focused = false;
	}

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

		return direction * _acceleration; // "walking"
	}

	private void StateController_PauseMenuState(bool isPaused)
	{
		_isPaused = isPaused;
	}

}