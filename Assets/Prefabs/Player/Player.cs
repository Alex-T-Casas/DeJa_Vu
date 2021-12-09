using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //Refreance scripts
    private LogActions actionLog;
    private MovementControler movmentComp;
    private InputActions inputActions;
    private Animator animator;

    //[SerializeField] Image recordingLayer;
    //[SerializeField] Image playingLayer;

    private void Awake()
    {
        inputActions = new InputActions();
    }

    void Start()
    {
        OnEnable();
        actionLog = GetComponent<LogActions>();
        movmentComp = GetComponent<MovementControler>();
        animator = GetComponent<Animator>();
        //Set inputs 
        inputActions.Gameplay.XMovement.performed += XMovementOnperformed;
        inputActions.Gameplay.XMovement.canceled += XMovementOncanceled;

        inputActions.Gameplay.YMovement.performed += YMovementOnperformed;
        inputActions.Gameplay.YMovement.canceled += YMovementOncanceled;

        inputActions.Gameplay.Jump.performed += JumpOnperformed;

        inputActions.Gameplay.Record.performed += RecordOnperformed;

        inputActions.Gameplay.Replay.performed += ReplayOnperformed;

        //recordingLayer.enabled = false;
        //playingLayer.enabled = false;
    }

    #region InputActions
    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void XMovementOncanceled(InputAction.CallbackContext obj)
    {
        movmentComp.SetMovementInputX(obj.ReadValue<float>());
    }

    private void XMovementOnperformed(InputAction.CallbackContext obj)
    {
        movmentComp.SetMovementInputX(obj.ReadValue<float>());
    }
    private void YMovementOncanceled(InputAction.CallbackContext obj)
    {
        movmentComp.SetMovementInputY(obj.ReadValue<float>());
    }

    private void YMovementOnperformed(InputAction.CallbackContext obj)
    {
        movmentComp.SetMovementInputY(obj.ReadValue<float>());
    }

    void JumpOnperformed(InputAction.CallbackContext ctx)
    {
        Debug.Log("Jump!");

        //movmentComp.Jump();
    }

    private void ReplayOnperformed(InputAction.CallbackContext obj)
    {
        actionLog.StartReplay();
        
        if (actionLog.isReplaying)
        {
            //playingLayer.enabled = true;
        }
        else
        {
            //playingLayer.enabled = false;
        }
    }

    private void RecordOnperformed(InputAction.CallbackContext obj)
    {
        if(actionLog.isRecording)
        {
            //recordingLayer.enabled = false;
            actionLog.EndRecording();
        }
        else
        {
            //recordingLayer.enabled = true;
            actionLog.StartRecording();
        }
    }

    void UpdateAnimationParamaters()
    {
        /*Vector3 PlayerFacingDir = movmentComp.GetPlayerDesiredLookDir();
        Vector3 PlayerMoveDir = movmentComp.GetPlayerDesiredMoveDir();
        Vector3 PlayerRight = transform.right;
        float ForwardDistribution = Vector3.Dot(PlayerFacingDir, PlayerMoveDir);
        float RightDistribution = Vector3.Dot(PlayerRight, PlayerMoveDir);*/
        animator.SetFloat("Y", movmentComp.MoveInputY);
        animator.SetFloat("X", movmentComp.MoveInputX);
    }
    #endregion

    void Update()
    {
        UpdateAnimationParamaters();
    }
}
