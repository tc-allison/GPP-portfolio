using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    PlayerManager playerManager;
    AnimatorManager animatorManager;
    InputManager inputManager;

    Vector3 moveDirection;
    Transform cameraObject;
    Rigidbody rb;

    public int jumpsRemaining = 0;
    public int maxJumps = 0;


    public float currentDoubleJumpTime = 0f;
    public float startingDoubleJumpTime = 10f;

    public float currentSpeedTime = 0f;
    public float startingSpeedTime = 5f;

    public bool speedPickedUp = false;
    public bool insideTrigger = false;
    public bool doorInteractTrigger = false;

    public bool isAttacking = false;

    public GameObject slashParticles;
    public GameObject djParticles;
    public GameObject speedParticles;


    [Header("Falling")]
    public float inAirTimer;
    public float leapingVelocity;
    public float fallingVelocity;
    public float rayCastHeightOffSet = 0.5f;
    public LayerMask groundLayer;



    [Header("Movement Flags")]
    public bool isSprinting;
    public bool isGrounded;
    public bool isJumping;

    [Header("Movement Speeds")]
    public float baseWalkSpeed = 5.0f;
    public float baseRunSpeed = 8.0f;
    public float baseSprintSpeed = 11.0f;
    public float walkingSpeed = 0f;
    public float runningSpeed = 0f;
    public float sprintingSpeed = 0f;
    public float rotationSpeed = 15.0f;
    public float walkPowerUpSpeed = 10f;
    public float runPowerUpSpeed = 16.0f;
    public float sprintPowerUpSpeed = 22.0f;

    

    [Header("Jump Speeds")]
    public float jumpHeight = 3f;
    public float gravityIntensity = -15f;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponent<AnimatorManager>();  
        inputManager = GetComponent<InputManager>();
        rb = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
        currentDoubleJumpTime = startingDoubleJumpTime;
        currentSpeedTime = startingSpeedTime;
        walkingSpeed = baseWalkSpeed;
        runningSpeed = baseRunSpeed;
        sprintingSpeed = baseSprintSpeed;
        slashParticles = GameObject.Find("SlashParticles");
        djParticles = GameObject.Find("Double Jump particles");
        speedParticles = GameObject.Find("Speed Boost particles");
        speedPickedUp = false;
    }

    public void HandleAllMovement()
    {
        HandleDoubleJumpTimer();
        HandleSpeedTimer();
        HandleFallingAndLanding();
        if (playerManager.isInteracting)
            return;
        HandleMovment();
        HandleRotaion();
        



    }
    private void HandleMovment()
    {
        if (isJumping)
            return;

        moveDirection = cameraObject.forward * inputManager.verticalInput;
        moveDirection = moveDirection + cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (isSprinting)
        {
            moveDirection = moveDirection * sprintingSpeed;
        }
        else
        {
            if (inputManager.moveAmount >= 0.5f)
            {
                moveDirection = moveDirection * runningSpeed;
            }
            else
            {
                moveDirection = moveDirection * walkingSpeed;
            }
        }




        Vector3 movementVelocity = moveDirection;
        rb.velocity = movementVelocity;
    }

    private void HandleRotaion()
    {
        if (isJumping)
            return;
        Vector3 targetDirection = Vector3.zero;

        targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection = targetDirection + cameraObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
            targetDirection = transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    private void HandleFallingAndLanding()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        rayCastOrigin.y = rayCastOrigin.y + rayCastHeightOffSet;

        if (!isGrounded && !isJumping)
        {
            if(!playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("2Hand-Sword-Fall", true);
            }

            inAirTimer = inAirTimer + Time.deltaTime;
            rb.AddForce(transform.forward * leapingVelocity);
            rb.AddForce(-Vector3.up * fallingVelocity * inAirTimer);
        }

        if (Physics.SphereCast(rayCastOrigin, 0.2f, -Vector3.up, out hit, groundLayer))
        {
            if (!isGrounded && !playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("2Hand-Sword-Land", true);
            }

            inAirTimer = 0;
            isGrounded = true;
            jumpsRemaining = maxJumps;
        }
        else
        {
            isGrounded= false;
        }
    }

    public void HandleJumping()
    {
        
        if (isGrounded || jumpsRemaining > 0) 
        {
            jumpsRemaining -= 1;
            animatorManager.animator.SetBool("isJumping", true);
            animatorManager.PlayTargetAnimation("2Hand-Sword-Jump", false);

            float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
            Vector3 playerVelocity = moveDirection;
            playerVelocity.y = jumpingVelocity;
            rb.velocity = playerVelocity;
            
        }
        
    }

    public void HandleDoubleJumpTimer()
    {
        if (maxJumps > 0) 
        {
            currentDoubleJumpTime -= 1 * Time.deltaTime;
            djParticles.GetComponent<ParticleSystem>().Play();
            //Debug.Log(currentDoubleJumpTime);

            if (currentDoubleJumpTime <= 0) 
            {
                maxJumps = 0;
                djParticles.GetComponent<ParticleSystem>().Stop();
                currentDoubleJumpTime = startingDoubleJumpTime;
            }
        }
   
    }

    public void HandleSpeedTimer()
    {
        if (speedPickedUp)
        {
            currentSpeedTime -= 1 * Time.deltaTime;
            speedParticles.GetComponent<ParticleSystem>().Play();
            //Debug.Log(currentSpeedTime);

            if (currentSpeedTime <= 0)
            {
                speedParticles.GetComponent<ParticleSystem>().Stop();
                walkingSpeed = baseWalkSpeed;
                runningSpeed = baseRunSpeed;
                sprintingSpeed = baseSprintSpeed;
                currentSpeedTime = startingSpeedTime;
                speedPickedUp = false;
            }
        }


    }



    public void HandleAttack()
    {
        if(playerManager.isInteracting || !isGrounded || isJumping) 
        {
            return;
        }
        else if (insideTrigger)
        {
            doorInteractTrigger = true;
            return;
        }
        animatorManager.PlayTargetAnimation("2Hand-Sword-Attack5", true);
        StartCoroutine(SlashCoroutine());
        StartCoroutine(AttackCooldown());

    }

    IEnumerator SlashCoroutine() 
    {
        yield return new WaitForSeconds(0.5f);
        slashParticles.GetComponent<ParticleSystem>().Play();
    }
    IEnumerator AttackCooldown()
    {
        isAttacking = true;
        yield return new WaitForSeconds(1);
        isAttacking = false;
    }



}
