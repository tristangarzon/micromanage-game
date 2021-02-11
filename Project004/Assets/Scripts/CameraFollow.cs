using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public void OnMouseDown()
    {

        //Setting the transform of the camera to the transform of the object clicked
        CameraController.instance.followTransform = transform;        
    }

}
