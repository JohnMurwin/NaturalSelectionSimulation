using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{

    public static FollowCamera instance;
    public Transform followTransform;
    private KeyCode _escape = KeyCode.Escape;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (followTransform != null)
        {
            transform.position = followTransform.position;
        }
        
        // put in the ability to stop following a clicked on object
        if (Input.GetKeyDown(_escape))
        {
            followTransform = null;
        }
    }
}
