using UnityEngine;
using System.Collections;

public class ModelScript : MonoBehaviour
{
    public float rotationSpeed = 1f;
    public float mouseSensitivity = 0.4f;
    private float rotationDamper = 5f;
    private Vector3 mouseReference;
    private Vector3 mouseOffset;
    private Vector3 rotation;
    private bool isRotating;

    void Update()
    {
        //check if is rotating
        if (isRotating)
        {
            //obtain the offset of the mouse
            mouseOffset = Input.mousePosition - mouseReference;
            //apply the rotation
            rotation.y = (mouseOffset.x + mouseOffset.y) * -mouseSensitivity;
            //rotate the object
            transform.Rotate(rotation);
            //store the mouse reference
            mouseReference = Input.mousePosition;
        }
        else
        {
            //obtain sign
            float sign = rotation.y <0? -1:1 ;
            //damp the rotation
            rotation.y -= rotationDamper * sign * Time.deltaTime;
            //check if it gets slower
            rotation.y = Mathf.Abs(rotation.y) <= 1 ? sign : rotation.y;
            //rotate the object
            transform.rotation *= Quaternion.AngleAxis(rotationSpeed * rotation.y, Vector3.up);
            
        }
    }

    void OnMouseDown()
    {
        //Set rotating to true
        isRotating = true;
        //store the mouse position in reference
        mouseReference = Input.mousePosition;
    }

    void OnMouseUp()
    {
        //stop rotating when mouse is up
        isRotating = false;
    }
}