using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawControl : MonoBehaviour
{

    [SerializeField] bool movedRight = false;
    [SerializeField] bool movedUp = false;
    [SerializeField] public bool moveComplete = false;
    [SerializeField] public bool canMove = true;
    public bool dropped = false;


    [SerializeField] private float moveSpeed = 1f;
    private float moveStop = 0f;
    [SerializeField] private float returnSpeed = 3f;
    [SerializeField] private GameObject dropPoint;

    private float xInput;
    private float yInput;

    private Rigidbody rb;
    private Animator anim;

    public GameObject keyR;
    public GameObject keyU;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        dropPoint.SetActive(false);
        keyR.SetActive(true);
        keyU.SetActive(true);
        canMove = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(moveComplete == false)
        {
            if(canMove)
            {
                if (movedRight != true)
                {
                    xInput = Input.GetAxisRaw("Horizontal");
                }
                if (movedUp != true)
                {
                    yInput = Input.GetAxisRaw("Vertical");
                }
            }

        }

       StartMovement();
       
    }

    private void FixedUpdate()
    {

        //diagonal movement
        if (xInput !=0 && yInput != 0)
        {
            xInput *= moveStop;
            yInput *= moveStop;
        }
     
        
        rb.velocity = new Vector3(xInput * moveSpeed, 0, yInput * moveSpeed);
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S))
        {
            rb.velocity = new Vector3(0, 0, 0);
        }

    }

    private void StartMovement()
    {
        if (canMove && movedRight == false)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                rb.velocity = Vector3.right * xInput * moveSpeed * Time.deltaTime;
                dropped = false;
            }
            if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
            {
                rb.velocity = Vector3.zero *  Time.deltaTime;
                movedRight = true;
                keyR.SetActive(false);
                dropped = false;
            }
        }
        if(canMove && movedUp == false)
        {
            if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                rb.AddForce(Vector3.right * yInput * moveSpeed);
                dropped = false;
            }
            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
            {
                rb.velocity = Vector3.zero;
                movedUp = true;
                keyU.SetActive(false);
                dropped = false;
            }
        }
        if (movedRight && movedUp == true)
        {
            moveComplete = true;
            ReturnMovement();
        }
    }



    private void ReturnMovement()
    {
        anim.Play("grab");
        StartCoroutine(MoveClaw());
        dropPoint.SetActive(true);

    }

    private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.CompareTag("Key") && moveComplete == true)
        {
            anim.Play("drop");

            moveComplete = false;
            movedRight = false;
            movedUp = false;
            dropPoint.SetActive(false);
            canMove = true;
            StopAllCoroutines();
            keyR.SetActive(true);
            keyU.SetActive(true);
            dropped = true;
            //could make another coroutine to stop the player from regaining control as soon as the claw hits the trigger?
        }
    }

    public Transform dropPosition;

    IEnumerator MoveClaw()
    {
        yield return new WaitForSeconds(3.5f);
        transform.position = Vector3.MoveTowards(transform.position, dropPosition.position, returnSpeed * Time.deltaTime);
    }
    
}
