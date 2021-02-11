using System.Collections;
using System.Collections.Generic;
using UnityEditor.Android;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public float movementSpeed;
    public float movementTime;
    public float rotationAmount;
    public float panBorderThickness;

    public Vector3 newPosition;
    public Quaternion newRotation;


  
    void Start()
    {
        newPosition = transform.position;
        newRotation = transform.rotation;
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


        //Smooths the movement of the camera
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);

        //Smooths the rotation of the camera
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
    }


    void HandlesMouseMovementInput()
    {
        //Moves the camera ("Mouse")
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

        //Smooths the movement of the camera
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
    }

}
