using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSelect : MonoBehaviour
{
    public void OnMouseDown(){
        FlyCamMod.instance.followTransform = transform;
    }
}
