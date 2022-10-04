using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private bool attacking;
    public float playerMaxHealth = 2000;
    public float playerHealth;
    public int keysCollected;
    public bool hasTreasure;

    private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float turnSpeed;
    
    private CharacterController controller;
    private Vector3 moveDirection;
    private Animator anim;

    Vector3 fallVector;

    SpiderController spiderController;
    WizardController wizardController;
    GuardController guardController;
    BossController bossController;

    // Start is called before the first frame update
    void Start()
    {
        hasTreasure = false;
        keysCollected = 0;
        attacking = false;
        playerHealth = playerMaxHealth;
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
            //Attack();
            StartCoroutine(Attack());
        }

        if (playerHealth <= 0)
        {
            // Debug.Log("Dead");
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


    IEnumerator Attack()
    {
        attacking = true;
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        attacking = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (attacking == true)
        {
            if (collision.gameObject.tag == "SpiderTag")
            {
                spiderController = collision.gameObject.GetComponent<SpiderController>();
                spiderController.TakeDamage();
            }
            if (collision.gameObject.tag == "WizardTag")
            {
                wizardController = collision.gameObject.GetComponent<WizardController>();
                wizardController.TakeDamage();
            }
            if (collision.gameObject.tag == "GuardTag")
            {
                guardController = collision.gameObject.GetComponent<GuardController>();
                guardController.TakeDamage();
            }
            if (collision.gameObject.tag == "BossTag")
            {
                bossController = collision.gameObject.GetComponent<BossController>();
                bossController.TakeDamage();
            }
        }
    }

    public void addKey()
    {
        keysCollected++;
    }
}
