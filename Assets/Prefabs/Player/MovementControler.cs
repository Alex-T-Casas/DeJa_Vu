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

    [Header("Physics")]
    public float Gravity = -9.81f;
    public float pushPower = 2.0f;

    [Header("Walking")]
    [SerializeField] float WalkingSpeed = 5f;
    //[SerializeField] float turnSmoothTime = 0.1f;
    //[SerializeField] float turnSmoothVelocity;
    [SerializeField] float RotationSpeed = 5f;
    public float MoveInputX;
    public float MoveInputY;
    public float MoveingSideways;

    [SerializeField] float jumpHeight = 2.0f;
    [SerializeField] Vector3 Velocity;
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

    /*public void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && IsOnGround())
        {
            Velocity.y += Mathf.Sqrt(jumpHeight * -2f * Gravity);
        }
    }*/
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

        CalculateWalkingVelocity();

        Vector3 MoveDir = GetPlayerDesiredMoveDir();

        Velocity.x = MoveDir.x * WalkingSpeed;
        Velocity.z = MoveDir.z * WalkingSpeed;
        if (Velocity.magnitude >= 0.1)
        {
            Quaternion GoalRotation = Quaternion.LookRotation(MoveDir, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, GoalRotation, Time.deltaTime * RotationSpeed);

            characterController.Move(Velocity * Time.deltaTime);
        }

        CheckFloor();
        FollowFloor();
        SnapShotPostitionAndRotation();
    }

    public Vector3 GetPlayerDesiredMoveDir()
    {
        return (MoveInputX * GetCameraRightDir() + MoveInputY * GetCameraForwardDir()).normalized;
    }

    void CalculateWalkingVelocity()
    {
        if(Input.GetKeyDown(KeyCode.Space) && IsOnGround())
        {
            Velocity.y += Mathf.Sqrt(jumpHeight * -2f * Gravity);
        }
        else if (IsOnGround() && Velocity.y < 0)
        {
            Velocity.y = 0f;
        }
        else
        {
            Velocity += Physics.gravity;
        }

        Velocity.x = GetPlayerDesiredMoveDir().x * WalkingSpeed;
        Velocity.z = GetPlayerDesiredMoveDir().z * WalkingSpeed;

        Vector3 PosXTrancePos = transform.position + new Vector3(TraceingDistance, 0.5f, 0f);
        Vector3 NegXTrancePos = transform.position + new Vector3(-TraceingDistance, 0.5f, 0f);
        Vector3 PosZTrancePos = transform.position + new Vector3(0f, 0.5f, TraceingDistance);
        Vector3 NegZTrancePos = transform.position + new Vector3(0f, 0.5f, -TraceingDistance);

        bool CanGoPosX = Physics.Raycast(PosXTrancePos, Vector3.down, TraceingDipth, GroundCheckMask);
        bool CanGoNegX = Physics.Raycast(NegXTrancePos, Vector3.down, TraceingDipth, GroundCheckMask);
        bool CanGoPosZ = Physics.Raycast(PosZTrancePos, Vector3.down, TraceingDipth, GroundCheckMask);
        bool CanGoNegZ = Physics.Raycast(NegZTrancePos, Vector3.down, TraceingDipth, GroundCheckMask);

        float xMin = CanGoNegX ? float.MinValue : 0f;
        float xMax = CanGoPosX ? float.MaxValue : 0f;
        float zMin = CanGoNegZ ? float.MinValue : 0f;
        float zMax = CanGoPosZ ? float.MaxValue : 0f;

        Velocity.x = Mathf.Clamp(Velocity.x, xMin, xMax);
        Velocity.z = Mathf.Clamp(Velocity.z, zMin, zMax);
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

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        //if no rigidbody is found or is not moveable
        if(body == null || body.isKinematic)
        {
            return;
        }

        //Preventing the pushing of obj below the player
        if(hit.moveDirection.y < -0.3f)
        {
            return;
        }

        //calcultate push dir from player move dir 
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        //Apply the push to the hit rigidbody
        body.velocity = pushDir * pushPower;
    }
}
