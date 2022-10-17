using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private bool attacking;
    public float playerMaxHealth = 10000;
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

    public AudioSource attackSound;

    GameManager gameManager;

    public GameObject loseScreen;

    public AudioSource loseSound;

    public AudioSource hitByRockSound;

    // Start is called before the first frame update
    void Start()
    {
        attacking = false;
        gameManager = GameObject.FindGameObjectWithTag("GameManagerTag").GetComponent<GameManager>();
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

        if (gameManager.isPlaying)
        {
            Move();
            Rotatee();
        }
        

        if (Input.GetKeyDown(KeyCode.Space) && gameManager.isPlaying && !attacking)
        {
            //Attack();
            StartCoroutine(Attack());
        }

        if (playerHealth <= 0 && gameManager.isPlaying)
        {
            StartCoroutine(LoseGame());
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
        RandomiseSound();
        attackSound.Play();
        yield return new WaitForSeconds(1);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "RockTag")
        {
            hitByRockSound.Play();
            playerHealth -= 10;
        }
        if (other.gameObject.tag == "FloorFireTag")
        {
            playerHealth -= 10;
        }
    }

    public void addKey()
    {
        keysCollected++;
    }
    void RandomiseSound()
    {
        attackSound.pitch = Random.Range(0.75f, 1.25f);
        attackSound.volume = Random.Range(0.75f, 1f);
    }


    IEnumerator LoseGame()
    {
        anim.Play("Die");
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        loseSound.Play();
        loseScreen.SetActive(true);
        gameManager.isPlaying = false;
        Time.timeScale = 0f;
    }
}
