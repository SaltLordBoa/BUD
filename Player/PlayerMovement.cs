using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("BaseMovement")]
    [SerializeField] bool canMove;
    [SerializeField] bool isMoving;
    [SerializeField] bool canClimb;
    bool canSprint;
    [SerializeField] bool isSprinting;
    [SerializeField] private float movementSpeed = 5f;
    private float baseSpeed;
    [SerializeField] private float sprintMultiplier = 15f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float distToGround = 1f;
    [SerializeField] private LayerMask groundLayer;

    private bool isGrounded;

    Vector3 moveDirection;
    [SerializeField] Transform orientation;
    int currentDirection = 1;
    int climbDirection = 1;

    private float xInput;
    private float yInput;

    private Rigidbody rb;
    private PlayerAnimation playerAnimation;
    public static PlayerMovement current;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerAnimation = GetComponent<PlayerAnimation>();
        canMove = true;
        canSprint = true;
        baseSpeed = movementSpeed;
    }

    void Update()
    {
        BaseMovement();
        OnSlope();
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if (!canMove)
        {
            playerAnimation.SetMoving(false);
        }

        if (canMove)
        {
            rb.velocity = new Vector3(xInput, rb.velocity.y, yInput);

            if (xInput != 0 || yInput != 0)
            {
                isMoving = true;
            }
            else
            {
                isMoving = false;
            }


        }
    }

    private void FixedUpdate()
    {
        CheckGround();

        //movement
        Vector3 horizontalDirection = transform.right * xInput;
        if (canClimb == true)
        {
            horizontalDirection = transform.up * xInput * climbDirection;
        }

    }

    float axisCombined;
    public void BaseMovement()
    {
        if (canMove)
        {
            xInput = Input.GetAxis("Horizontal") * movementSpeed;
            yInput = Input.GetAxis("Vertical") * movementSpeed;
            axisCombined = xInput / yInput;

            //rotation
            Vector3 movement = new Vector3(xInput + 0.001f, 0.0f, yInput).normalized;
            Quaternion newRotation = Quaternion.LookRotation(movement);
            //rotates player object on Y-axis 
            if (axisCombined >= 0.1f || axisCombined <= 0.1f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, 20 * Time.deltaTime);
            }

            moveDirection = orientation.forward * yInput * xInput;
            slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
            
            if (canSprint == true && isMoving == true && isGrounded == true && Input.GetKeyDown(KeyCode.LeftShift))
            {
                isSprinting = true;
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                isSprinting = false;               
            }


            if (isSprinting == true)
            {
                movementSpeed = sprintMultiplier;
            }
            else
            {
                movementSpeed = baseSpeed;
            }
        }

        //animations    
        if (xInput != 0 || yInput != 0)
        {
            playerAnimation.SetMoving(true);
        }
        else
        {
            playerAnimation.SetMoving(false);
        }
        if((xInput !=0 || yInput !=0) && isSprinting == true)
        {
            playerAnimation.SetSprinting(true);
        }
        else
        {
            playerAnimation.SetSprinting(false);
        }


        //not moving
        if (!canMove)
        {
            return;
        }
    }

    public void SetMovement(bool value)
    {
        canMove = value;
        rb.velocity = Vector3.zero;
        isMoving = false;
        isSprinting = false;
    }

    public void SetSprint(bool value)
    {
        canSprint = value;
    }

    IEnumerator EnableCanMove(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canMove = true;
    }

    private void Jump()
    {
        if (isGrounded && canMove)
        {
            rb.velocity += (Vector3.up * jumpForce);
            isGrounded = false;
            playerAnimation.SetJumping(true);
            return;
        }
    }

    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    public float playerHeight = 2f;
    Vector3 slopeMoveDirection;

    private void CheckGround()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, distToGround + 0.1f);

        if (isGrounded && !OnSlope())
        {
            rb.AddForce(slopeMoveDirection.normalized * movementSpeed, ForceMode.Acceleration);
        }
        else if (isGrounded && OnSlope())
        {
            rb.AddForce(slopeMoveDirection.normalized * movementSpeed, ForceMode.Acceleration);
        }
        if (isGrounded)
        {
            playerAnimation.SetFalling(false);
            playerAnimation.SetJumping(false);
        }
        else
        {
            playerAnimation.SetFalling(true);
            transform.parent = null;
        }

    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.5f))
        {
            if(slopeHit.normal != Vector3.up)
            {
                return true;
            }
            return false;
        }
        return false;
    }

    public void SetClimb(bool value)
    {
        canClimb = value;
        if (canClimb == true)
        {
            rb.useGravity = false;
            climbDirection = currentDirection;
        }
        else
        {
            rb.useGravity = true;
        }
    }

  
}
