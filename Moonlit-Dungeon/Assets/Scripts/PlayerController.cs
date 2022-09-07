using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int playerHealth;
    public int keysCollected;

    private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float turnSpeed;
    
    private CharacterController controller;
    private Vector3 moveDirection;
    private Animator anim;

    Vector3 fallVector;


    // Start is called before the first frame update
    void Start()
    {
        playerHealth = 2000;
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        fallVector = Vector3.zero;

        if (controller.isGrounded == false)
        {
            fallVector += Physics.gravity;
        }

        controller.Move(fallVector * Time.deltaTime);

        Move();
        Rotatee();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }

        if (playerHealth <= 0)
        {
            Debug.Log("Dead");
        }
        
    }

    private void Move()
    {

        float moveZ = Input.GetAxis("Vertical");

        moveDirection = new Vector3(0, 0, moveZ);
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= walkSpeed;

        if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
        {
            Walk();
        }
        else if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
        {
            Run();
        }
        else if (moveDirection == Vector3.zero)
        {
            Idle();
        }
        

        moveDirection *= moveSpeed;


        controller.Move(moveDirection * Time.deltaTime);
    }

    private void Idle()
    {
        anim.SetFloat("Speed", 0, 0.05f, Time.deltaTime);
    }

    private void Run()
    {
        moveSpeed = runSpeed;
        anim.SetFloat("Speed", 1, 0.05f, Time.deltaTime);
    }

    private void Walk()
    {
        moveSpeed = walkSpeed;
        anim.SetFloat("Speed", 0.5f, 0.05f, Time.deltaTime);
    }


    private void Rotatee()
    {
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
        }

        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            transform.Rotate(Vector3.up, -turnSpeed * Time.deltaTime);
        }
    }

    private void Attack()
    {
        anim.SetTrigger("Attack");
    }
}
