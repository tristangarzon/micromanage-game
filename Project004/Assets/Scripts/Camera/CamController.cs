using System.Collections;
using System.Collections.Generic;
using UnityEditor.Android;
using UnityEngine;

public class CamController : MonoBehaviour
{
    [Header("Main Camera")]
    public Transform cameraTransform;

    [Header("Movement")]
    public float movementSpeed;
    public float movementTime;
    public Vector3 newPosition;
    public Vector3 dragStartPosition;
    public Vector3 dragCurrentPosition;

    [Header("Rotation")]
    public float rotationAmount;
    public Quaternion newRotation;
    public Vector3 rotateStartPosition;
    public Vector3 rotateCurrentPosition;

    [Header("Zoom")]
    public Vector3 zoomAmount;
    public float minZoom;
    public float maxZoom;
    public Vector3 newZoom;

    [Header("Pan")]
    public float panBorderThickness;
    public Vector2 panLimit;
   
  
    void Start()
    {
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    void Update()
    {
        HandleKeyboardMovementInput();
        HandlesMouseMovementInput();
    }

    void HandleKeyboardMovementInput()
    {
        //Moves the camera ("WASD" or "Arrow Keys")
        //Up
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            newPosition += (transform.forward * movementSpeed);
        }
        //Down
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            newPosition += (transform.forward * -movementSpeed);
        }
        //Right
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            newPosition += (transform.right * movementSpeed);
        }
        //Left
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition += (transform.right * -movementSpeed);
        }

        /*
        //Rotate the Camera ("Q" & "W")
        //Clockwise
        if (Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }
        //Counter-Clockwise
        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }
        */

        //Smooths the movement of the camera
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);

        //Smooths the rotation of the camera
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
    }


    void HandlesMouseMovementInput()
    {
        //Zoom
        if(Input.mouseScrollDelta.y != 0)
        {
            if (Input.mouseScrollDelta.y > 0 && newZoom.z < maxZoom)
            {
                newZoom += Input.mouseScrollDelta.y * zoomAmount;
            }
            if (Input.mouseScrollDelta.y < 0 && newZoom.z > minZoom)
            {
                newZoom += Input.mouseScrollDelta.y * zoomAmount;
            }
        }

        //Moves the camera edge("Mouse")
        //Up
        if (Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            newPosition += (transform.forward * movementSpeed);
        }
        //Down
        if (Input.mousePosition.y <= panBorderThickness)
        {
            newPosition += (transform.forward * -movementSpeed);
        }
        //Right
        if (Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            newPosition += (transform.right * movementSpeed);
        }
        //Left
        if (Input.mousePosition.x <= panBorderThickness)
        {
            newPosition += (transform.right * -movementSpeed);
        }

        //Moves the camera drag("Mouse")
        if (Input.GetMouseButtonDown(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if(plane.Raycast(ray, out entry))
            {
                dragStartPosition = ray.GetPoint(entry);
            }
        }
        if (Input.GetMouseButton(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry))
            {
                dragCurrentPosition = ray.GetPoint(entry);

                newPosition = transform.position + dragStartPosition - dragCurrentPosition; 
            }
        }

        //Rotates the camera("Mouse")
        if (Input.GetMouseButtonDown(2))
        {
            rotateStartPosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(2))
        {
            rotateCurrentPosition = Input.mousePosition;

            float dx = (rotateCurrentPosition - rotateStartPosition).x * rotationAmount;
            float dy = (rotateCurrentPosition - rotateStartPosition).y * rotationAmount;
            float dz = (rotateCurrentPosition - rotateStartPosition).z * rotationAmount;


            transform.rotation *= Quaternion.Euler(new Vector3(0, dx, dz));

        }

        //Limits how far the camera is able to move/zoom
        newPosition.x = Mathf.Clamp(newPosition.x, -panLimit.x, panLimit.x);
        newPosition.z = Mathf.Clamp(newPosition.z, -panLimit.y, panLimit.y);
     

        //Smooths the movement of the camera
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
    }

}
