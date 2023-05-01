using RPGCharacterAnims.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerLocomotion playerLocomotion;
    AnimatorManager animatorManager;

    public Vector2 movementInput;
    public Vector2 cameraInput;

    public float cameraInputHorizontal;
    public float cameraInputVertical;

    public float moveAmount;
    public float verticalInput;
    public float horizontalInput;

    public bool shoulderL_Input;
    public bool A_Input;
    public bool X_Input;
    

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        playerLocomotion= GetComponent<PlayerLocomotion>();
    }
    private void OnEnable()
    {
        if (playerControls == null) 
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            playerControls.Actions.Sprinting.performed += i => shoulderL_Input = true;
            playerControls.Actions.Sprinting.canceled += i => shoulderL_Input = false;
            playerControls.Actions.Jump.performed += i => A_Input = true;
            playerControls.Actions.Attack.performed += i => X_Input = true;
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleSprintingInput();
        HandleJumpingInput();
        HandleAttackInput();
    }

    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        cameraInputHorizontal = cameraInput.x;
        cameraInputVertical = cameraInput.y;


        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimatorValues(0, moveAmount, playerLocomotion.isSprinting);
    }

    private void HandleSprintingInput()
    {
        if (shoulderL_Input && moveAmount > 0.5f)
        {
            playerLocomotion.isSprinting = true;
        }
        else
        {
            playerLocomotion.isSprinting = false;
        }
    }

    private void HandleJumpingInput()
    {
        if (A_Input)
        {
            A_Input= false;
            playerLocomotion.HandleJumping();
        }
    }

    private void HandleAttackInput()
    {
        if (X_Input) 
        {
            X_Input = false;
            playerLocomotion.HandleAttack();
        }
    }



}
