using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControler : MonoBehaviour
{
    [Header("Walking")]
    [SerializeField] float WalkingSpeed = 5f;
    [SerializeField] float turnSmoothTime = 0.1f;
    [SerializeField] float turnSmoothVelocity;
    [SerializeField] float RotationSpeed = 5f;
    public float MoveInputX;
    public float MoveInputY;
    public float MoveingSideways;

    Vector3 Velocity;
    CharacterController characterController;
    [SerializeField] Transform Cam;


    public void SetMovementInputX(float inputVal)
    {
        MoveInputX = inputVal;
        Debug.Log($"Move X is: {MoveInputX}");

        /*if (MoveInputX != 0)
        {
            MoveingSideways = 1;
        }
        else
        {
            MoveingSideways = 0;
        }*/
    }
    public void SetMovementInputY(float inputVal)
    {
        MoveInputY = inputVal;
        Debug.Log($"Move Y is: {MoveInputY}");
    }


    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Vector3 MoveDir = GetPlayerDesiredMoveDir();
        Vector3 AvatarDir = transform.forward;

        
        Velocity = MoveDir * WalkingSpeed;
        if (Velocity.magnitude >= 0.1)
        {
            Quaternion GoalRotation = Quaternion.LookRotation(MoveDir, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, GoalRotation, Time.deltaTime * RotationSpeed);

            //float targetAngle = Mathf.Atan2(Velocity.x, Velocity.y) * Mathf.Rad2Deg + Cam.eulerAngles.y;
            //float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            //transform.rotation = Quaternion.Euler(0f, angle, 0f);

            characterController.Move(Velocity * Time.deltaTime);
        }

    }

    /*internal Vector3 GetPlayerDesiredLookDir()
    {
        Ray CursorToWorldRay = Camera.main.ScreenPointToRay(new Vector3(0.5F, 0.5F, 0));
        float height = CursorToWorldRay.origin.y - transform.position.y;
        float length = height / Vector3.Dot(new Vector3(0, -1, 0), CursorToWorldRay.direction);
        Vector3 LookAtLoc = CursorToWorldRay.origin + CursorToWorldRay.direction * length;
        Vector3 LookAtDir = (LookAtLoc - transform.position).normalized;
        return LookAtDir;
    }*/

    public Vector3 GetPlayerDesiredMoveDir()
    {
        return (MoveInputX * GetCameraRightDir() + MoveInputY * GetCameraForwardDir()).normalized;
    }

    Vector3 GetCameraRightDir()
    {
        return Camera.main.transform.right;
    }

    Vector3 GetCameraForwardDir()
    {
        Vector3 CameraRight = GetCameraRightDir();
        Vector3 UpVector = Vector3.up;
        return -Vector3.Cross(UpVector, CameraRight);
    }
}
