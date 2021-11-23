using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //Refreance scripts
    private MovementControler movmentComp;
    private InputActions inputActions;
    private Animator animator;

    private void Awake()
    {
        inputActions = new InputActions();
    }

    void Start()
    {
        OnEnable();
        movmentComp = GetComponent<MovementControler>();
        animator = GetComponent<Animator>();
        //Set inputs 
        inputActions.Gameplay.XMovement.performed += XMovementOnperformed;
        inputActions.Gameplay.XMovement.canceled += XMovementOncanceled;

        inputActions.Gameplay.YMovement.performed += YMovementOnperformed;
        inputActions.Gameplay.YMovement.canceled += YMovementOncanceled;
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
