using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSelect : MonoBehaviour
{
    public void OnMouseDown(){
        GlobalCamera.instance.followTransform = transform;
    }
}
