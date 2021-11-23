using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_FPS_Comp : MonoBehaviour
{
    [SerializeField] float turnSpeed = 4.0f;
    [SerializeField] float moveSpeed = 2.0f;
    [SerializeField] float minTurnAngle = -90.0f;
    [SerializeField] float maxTurnAngle = 90.0f;
    [SerializeField] float rotX;

    void MouseAiming()
    {
        // get the mouse inputs
        float y = Input.GetAxis("Mouse X") * turnSpeed;
        rotX += Input.GetAxis("Mouse Y") * turnSpeed;
        // clamp the vertical rotation
        rotX = Mathf.Clamp(rotX, minTurnAngle, maxTurnAngle);
        // rotate the camera
        transform.eulerAngles = new Vector3(-rotX, transform.eulerAngles.y + y, 0);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MouseAiming();
    }
}
