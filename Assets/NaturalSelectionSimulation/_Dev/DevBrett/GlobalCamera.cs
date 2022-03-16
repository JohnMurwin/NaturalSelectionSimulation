using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalCamera : MonoBehaviour
{

    public Transform cameraTransform;
    public Transform followTransform;
    public static GlobalCamera instance;
    public Quaternion newRotation;

    private float movementSpeed = .01f;
    private float movementTime = 2f;
    private float rotationAmount = .1f;    

    public Vector3 newPosition;
    public Vector3 newZoom;
    public Vector3 zoomAmount;
    public Vector3 rotateStartPosition;
    public Vector3 rotateEndPosition;

    // the keys that control the movement of the camera
    private KeyCode _down = KeyCode.DownArrow;
    private KeyCode _up = KeyCode.UpArrow;
    private KeyCode _left = KeyCode.LeftArrow;
    private KeyCode _right = KeyCode.RightArrow;
    private KeyCode _escape = KeyCode.Escape;
    private KeyCode _rotationRight = KeyCode.R;
    private KeyCode _rotationLeft = KeyCode.E;
    private KeyCode _zoomIn = KeyCode.W;
    private KeyCode _zoomOut = KeyCode.Q;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    // This is what handels the camera being able to shift between following a moving object
    // and the global camera view
    void Update()
    {
        if (followTransform != null)
        {
           transform.position = followTransform.position; 
        } else {
            CameraMovement();
        }

        // put in the ability to stop following a clicked on object
        if(Input.GetKeyDown(_escape)){
            followTransform = null;
        }
    }


    // sets up all the key controls for moving the camera
    void CameraMovement()
    {
        if(Input.GetKey(_down)){
            newPosition += (transform.forward * movementSpeed);
        }

        if(Input.GetKey(_up)){
            newPosition += (transform.forward * -movementSpeed);
        }

        if(Input.GetKey(_right)){
            newPosition += (transform.right * -movementSpeed);
        }

        if(Input.GetKey(_left)){
            newPosition += (transform.right * movementSpeed);
        }

        if(Input.GetKey(_rotationRight)){
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount) ;
        }

        if(Input.GetKey(_rotationLeft)){
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount) ;
        }

        if(Input.GetKey(_zoomOut)){
            newZoom += zoomAmount;
        }

        if(Input.GetKey(_zoomIn)){
            newZoom -= zoomAmount;
        }

        // makes all the camera movements smooth
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
    }
}
