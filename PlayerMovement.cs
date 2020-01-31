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

    private Vector3 warpPosition = Vector3.zero;

    [Header("Movement Variables")]
    [SerializeField] float playerSpeed;
    [SerializeField] float playerAirSpeed;
    [SerializeField] float playerGravity;
    [SerializeField] float jumpHeight;


    public GameObject fireworks;

    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        HorizontalInput = Input.GetAxis("Horizontal");
        VerticalInput = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Jump"))
            calculateJump();

        if (Input.GetKeyDown(KeyCode.Alpha1))
            Instantiate(fireworks, transform.position, Quaternion.identity);
    }

    private void FixedUpdate()
    {
        calculatePlayerMovement();
    }

    private void calculateJump()
    {
        inputVector.y = jumpHeight;
    }

    public void changePosition(Vector3 t)
    {
        warpPosition = t;
    }


    private void calculatePlayerMovement()
    {
        if (controller.isGrounded)
        {
            Vector3 x = gameObject.transform.forward * VerticalInput * playerSpeed;
            Vector3 z = gameObject.transform.right * HorizontalInput * playerSpeed;

            Vector3 normal = (x + z).normalized;
            inputVector.x = normal.x * playerSpeed;
            inputVector.z = normal.z * playerSpeed;
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
