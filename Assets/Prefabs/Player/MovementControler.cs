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
    
    public float MoveInputX;
    public float MoveInputY;

    Vector3 Velocity;
    CharacterController characterController;
    [SerializeField] Transform Cam;


    public void SetMovementInputX(float inputVal)
    {
        MoveInputX = inputVal;
    }
    public void SetMovementInputY(float inputVal)
    {
        MoveInputY = inputVal;
    }


    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Velocity = GetPlayerDesiredMoveDir();

        if (Velocity.magnitude >= 0.1)
        {
            float targetAngle = Mathf.Atan2(Velocity.x, Velocity.y) * Mathf.Rad2Deg + Cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move(moveDir.normalized * WalkingSpeed * Time.deltaTime);
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
        return new Vector3(MoveInputX, 0f, -MoveInputY).normalized;
    }
}
