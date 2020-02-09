using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private float HorizontalInput;
    private float VerticalInput;
    private float JumpInput;
    private Vector3 inputVector;
    private float currentCollisionAngle;

    private Vector3 warpPosition = Vector3.zero;

    [Header("Movement Variables")]
    [SerializeField] float playerSpeed;
    [SerializeField] float playerAirSpeed;
    [SerializeField] float sprintSpeed;
    [SerializeField] float playerGravity;
    [SerializeField] float jumpHeight = 6;
    [SerializeField] float fallMultiplier;
    [SerializeField] float maximumPlayerJumpAngle = 30f;

    [Header("Sprint Variables")]
    [SerializeField] public float sprintTime;
    [SerializeField] float sprintRegen;
    [SerializeField] float highJumpAmount;

    [Header("Events")]
    [SerializeField] GameEvent isSprinting;

    private float currentSprint = 1;
    public float currentSprintRemaining;


    public GameObject fireworks;

    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        currentSprintRemaining = sprintTime;
    }

    void Update()
    {
        HorizontalInput = Input.GetAxis("Horizontal");
        VerticalInput = Input.GetAxis("Vertical");

        var movementVector = new Vector2(HorizontalInput, VerticalInput);

        if(currentSprintRemaining > 0)
        {
            if(movementVector.magnitude > 0)
            {
                currentSprint = 1 + Input.GetAxis("Sprint") * sprintSpeed;
                if (Input.GetButton("Sprint"))
                {
                    currentSprintRemaining -= Time.deltaTime;
                }
            }
        }
        else
        {
            currentSprint = 1;
            currentSprintRemaining = 0;
        }

        if (!Input.GetButton("Sprint") && currentSprintRemaining < sprintTime)
        {
            currentSprintRemaining += Time.deltaTime * sprintRegen;
        }

        if(currentSprintRemaining >= sprintTime)
        {
            currentSprintRemaining = sprintTime;
        }

        if(controller.velocity.y < 0)
        {
            inputVector += Vector3.down * playerGravity * (fallMultiplier - 1) * Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
            calculateJump();

    }

    private void OnControllerColliderHit(ControllerColliderHit hitObject)
    {
        currentCollisionAngle = Vector3.Angle(Vector3.up, hitObject.normal);
    }

    private void FixedUpdate()
    {
        calculatePlayerMovement();
    }

    private void calculateJump()
    {

        if (canJump())
        {

            if (Input.GetButton("Crouch"))
            {
                inputVector.y = jumpHeight * highJumpAmount;
            }
            else
            {
                inputVector.y = jumpHeight;

            }
        }
    }

    private bool canJump()
    {
        if (controller.isGrounded && currentCollisionAngle < maximumPlayerJumpAngle)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void changePosition(Vector3 t)
    {
        warpPosition = t;
    }


    private void calculatePlayerMovement()
    {

        if (controller.isGrounded)
        {
            Vector3 x = gameObject.transform.forward * VerticalInput * (playerSpeed);
            Vector3 z = gameObject.transform.right * HorizontalInput * (playerSpeed);

            Vector3 normal = (x + z).normalized;
            inputVector.x = normal.x * (playerSpeed * currentSprint);
            inputVector.z = normal.z * (playerSpeed * currentSprint);
        }
        else
        {
            Vector3 x = gameObject.transform.forward * VerticalInput * playerAirSpeed;
            Vector3 z = gameObject.transform.right * HorizontalInput * playerAirSpeed;

            Vector3 normal = (x + z).normalized;
            inputVector.x = normal.x * playerAirSpeed;
            inputVector.z = normal.z * playerAirSpeed;
            inputVector.y -= playerGravity * Time.deltaTime;
        }

        
        controller.Move(inputVector * Time.deltaTime);

        if(warpPosition != Vector3.zero)
        {
            transform.position = warpPosition;
            warpPosition = Vector3.zero;
        }
    }
}
