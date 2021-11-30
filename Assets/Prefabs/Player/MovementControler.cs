using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControler : MonoBehaviour
{
    [Header("Ground Check")]
    [SerializeField] Transform GroundCheck;
    [SerializeField] float GroundCheckRadius = 0.1f;
    [SerializeField] float TraceingDistance = 1f;
    [SerializeField] float TraceingDipth = 0.8f;
    [SerializeField] LayerMask GroundCheckMask;

    Transform currentFloor;
    Vector3 PreviousWorldPos;
    Vector3 PreviousFloorLocalPos;
    Quaternion PreviousWorldRot;
    Quaternion PreviousFloorLocalRot;

    float Gravity = -9.81f;

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

    void CheckFloor()
    {
        Collider[] cols = Physics.OverlapSphere(GroundCheck.position, GroundCheckRadius, GroundCheckMask);
        if (cols.Length != 0)
        {
            if (currentFloor != cols[0].transform)
            {
                currentFloor = cols[0].transform;
                SnapShotPostitionAndRotation();

            }
        }
    }

    void SnapShotPostitionAndRotation()
    {
        PreviousWorldPos = transform.position;
        PreviousWorldRot = transform.rotation;
        if (currentFloor != null)
        {
            PreviousFloorLocalPos = currentFloor.InverseTransformPoint(transform.position);
            PreviousFloorLocalRot = Quaternion.Inverse(currentFloor.rotation) * transform.rotation;
        }
    }

    bool IsOnGround()
    {
        return Physics.CheckSphere(GroundCheck.position, GroundCheckRadius, GroundCheckMask);
    }

    void FollowFloor()
    {
        if (currentFloor)
        {
            Vector3 DeltaMove = currentFloor.TransformPoint(PreviousFloorLocalPos) - PreviousWorldPos;
            Velocity += DeltaMove / Time.deltaTime;

            Quaternion DestinationRot = currentFloor.rotation * PreviousFloorLocalRot; // we are adding
            Quaternion DeltaRot = Quaternion.Inverse(PreviousWorldRot) * DestinationRot;
            transform.rotation = transform.rotation * DeltaRot;
        }
    }

    private void Update()
    {
        Vector3 MoveDir = GetPlayerDesiredMoveDir();
        Vector3 AvatarDir = transform.forward;

        Velocity.y += Gravity * Time.deltaTime;

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

        CheckFloor();
        FollowFloor();
        SnapShotPostitionAndRotation();
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
