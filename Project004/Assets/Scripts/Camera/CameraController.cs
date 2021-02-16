using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public Transform cameraTransform;
    public Transform followTransform;

    //Speed of the camera
    public float movementSpeed;
    //Movement delay of the camera
    public float movementTime;
    //Normal moving speed of the camera
    public float normalSpeed;
    //Var that will double the cameras normal speed
    public float fastSpeed;
    //The amount inwhich we want to rotate our camera
    public float rotationAmount;
    //The amount in which the camera will zoom in/out
    public Vector3 zoomAmount;

    //Mouse Zoom
    public float maxHeightY = 70;
    public float minHeightY = 1;
    public float zoomIncrease = 5;

 


    public Vector3 newPosition;
    public Quaternion newRotation;
    public Vector3 newZoom;

    //Mouse Drag on screen
    //Vector3's that will hold the position of our mouse
    public Vector3 dragStartPosition;
    public Vector3 dragCurrentPosition;
    //Mouse Rotation
    //Vector2's that will hold the position of our mouse 
    public Vector2 rotateStartPosition;
    public Vector2 rotateCurrentPosition;

   
    void Start()
    {
        instance = this;

        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    
    void Update()
    {
        //When clicked an object that allows for the followTransform, it will follow the object
        if (followTransform != null)
        {
            transform.position = followTransform.position;
        }
        else
        {
           
            HandleMovementInput();
            HandleMouseInput();
        }

        //If Esc Key is pressed it will stop following the object
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            followTransform = null;
        }

    }


    //Method that handles the movement of the camera with the mouse
    void HandleMouseInput()
    {
        

        //Keeps the camera confined to the set minHeight/maxHeight
        if (newZoom.y >= maxHeightY)
        {
            newZoom.y = maxHeightY;
            zoomAmount.z = 0;


        }
        else if (newZoom.y <= minHeightY)
        {
            newZoom.y = minHeightY;
            zoomAmount.z = 0;

        }
        else if (newZoom.y < maxHeightY || newZoom.y > minHeightY)
        {
            zoomAmount.z = zoomIncrease;


        }

        //Zooms the camera using the scroll wheel of the mouse
        if (Input.mouseScrollDelta.y != 0)
        {
            newZoom += Input.mouseScrollDelta.y * zoomAmount;
        }




        //Cast a ray to the plane, checks if left mouse point is pressed and will drag the plane 
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

        //Rotates the camera using the middle mouse button
        //Checks if the middle mouse button is pressed
        if (Input.GetMouseButtonDown(2))
        {
            rotateStartPosition = Input.mousePosition;
        }
        //Checks if the middle mouse button is held down
        if (Input.GetMouseButton(2))
        {
            rotateCurrentPosition = Input.mousePosition;

            float dx = (rotateCurrentPosition - rotateStartPosition).x * rotationAmount;
            float dy = (rotateCurrentPosition - rotateStartPosition).y * rotationAmount;

            //Y Rotation
            transform.rotation *= Quaternion.Euler(new Vector3(0, dx, 0));

            
        }

        //Smooths the movement of the camera
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
    }



    //Method that handles the movement of the camera with the keyboard
    void HandleMovementInput()
    {
        //When Shift is pressed, it will double the cameras normal movement speed
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = fastSpeed;
        }
        else
        {
            movementSpeed = normalSpeed;
        }

        //"WASD" & "Arrow Key" Movement
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            newPosition += (transform.forward * movementSpeed);
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            newPosition += (transform.forward * -movementSpeed);
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition += (transform.right * -movementSpeed);
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            newPosition += (transform.right * movementSpeed);
        }

        /*
        //"Q" & "E" will rotate the camera 
        if(Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }

        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }
        */

        /*
        //"R" & "F" will zoom the camera in/out
        if (Input.GetKey(KeyCode.R))
        {
            newZoom += zoomAmount;
        }

        if (Input.GetKey(KeyCode.F))
        {
            newZoom -= zoomAmount;
        }
        */

        //Smooths the movement of the camera
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        //Smooths the rotation of the camera
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
        //Smooths the zoom of the camera
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime); 

    }
}
