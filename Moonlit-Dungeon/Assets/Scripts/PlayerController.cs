using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// this script controls the player and all the attributes associated to it
/// </summary>
public class PlayerController : MonoBehaviour
{
    // this states if the player can attack or not.
    // It is switched on and off at time intervals so that the plyar doesnt attack too much at once
    private bool attacking;
    // this is the max health the player can be at. 
    public float playerMaxHealth = 250;
    // this is the current helath of the player at any given time
    public float playerHealth;
    // these are thenumber of keys collected in a level
    public int keysCollected;
    // this states if the player has the treause chest
    public bool hasTreasure;
    // this is speed of the player at any givern time, and is set in the inspector (Unity)
    private float moveSpeed;
    // this is the speed the player walks at
    [SerializeField] private float walkSpeed;
    // this is the acccelerated speed the play can run at when holding down shift
    [SerializeField] private float runSpeed;
    // this is the speed the player turns at.
    [SerializeField] private float turnSpeed;
    // this is a reference to the controller compoenent (Unitys' charatcer controller)
    // of the player. This is used to control the player objects movement
    private CharacterController controller;
    // this is the direciton of movement that the charater controller will use to move
    private Vector3 moveDirection;
    // this a refrence to the animator compoenent of the player, which will be used to contorl the players animations
    private Animator anim;
    // this the vector that the player falls at (used to add gravity to the character controller)
    Vector3 fallVector;
    // a reference to each and every enemies scripts. So that the player can do damage to them
    SpiderController spiderController;
    WizardController wizardController;
    GuardController guardController;
    BossController bossController;
    // this is the attacking sword sound that will play every time the player attacks
    public AudioSource attackSound;
    // this is a refrence to the gamemanager script just so the gameManager can know when the player has treause or is dead etc.
    GameManager gameManager;
    // this is refernce to the lose screen that appears when the player loses all health
    public GameObject loseScreen;
    // and this is the sound that plays when that happens
    public AudioSource loseSound;
    // this is the sound that the player plays whens its hit by one of those rocks the guard enemies throw
    public AudioSource hitByRockSound;
    // this is the damage from the special attacks the plaeyer gets, like the floor fire and rocks
    private int specialAttackDamage = 10;

    // Start is called before the first frame update
    void Start()
    {
        // this is the perfect ratio I found to work between the walk and turn speed of the player.
        // Doing this will mean that by changing just the walk speed, all the other speeds will adjust
        turnSpeed = 134 * walkSpeed;
        // same for the runspeed. this is the perfect ratio
        runSpeed = 2.5f * walkSpeed;
        // set the attacking to false so that the player isnt attacking right away
        attacking = false;
        // get the gamemanger script component from the object with the GameManagerTag in the heirachy
        gameManager = GameObject.FindGameObjectWithTag("GameManagerTag").GetComponent<GameManager>();
        // the player doesnt have treasure at the start because it hasnt won yet
        hasTreasure = false;
        // set the number of keys collected to 0
        keysCollected = 0;
        // set the starting health of the player to be the max health it can be at
        playerHealth = playerMaxHealth;
        // get the charcaetr controller compoenent off this gameobject this script is attached to (the player)
        controller = GetComponent<CharacterController>();
        // in the same eway as above, get the animator compoenent of the player 
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // set the fall vector to null so the player isnt constantly falling
        fallVector = Vector3.zero;

        // if the player contorller is detected as not touching the ground then:
        if (controller.isGrounded == false)
        {
            fallVector += Physics.gravity; // inscrease the fall vector by gravity
        }
        // move the controller by the fall vector times time. (if the vector is 0, then it wont move at all)
        controller.Move(fallVector * Time.deltaTime);
        // if the isPlaying boolean in the gameManager is set to true then this
        // means the game isnt in a menu and active so:
        if (gameManager.isPlaying)
        {
            Move(); // move the player based on input
            Rotatee(); // rotate the player based on input
        }

        // if the fire1 key (the deufult fire key on all controllers) is being
        // pressed down while the game is active and the player isnt already attacking then attack:
        if (Input.GetButtonDown("Fire1") && gameManager.isPlaying && !attacking)
        {
            StartCoroutine(Attack()); // start the attack coroutine whcih attacks
                                      // the enemy the playrr is attacking and plays the attack animation
        }
        // if the health of the player is less than or equal to 0 (meaning the player is dead)
        // and the isPlaying boolean is true still, then lose the game
        if (playerHealth <= 0 && gameManager.isPlaying)
        {
            StartCoroutine(LoseGame()); // pop up th lose game screen and play the appropriate sound
        }
        
    }
    /// <summary>
    /// this script handles the movement of the player based on the vertical input keys
    /// </summary>
    private void Move()
    {
        // store the move value (derived from the vertical keys) in moveZ
        float moveZ = Input.GetAxis("Vertical");
        // make the direction of movement equal to that moveZ
        moveDirection = new Vector3(0, 0, moveZ);
        // then turn that variabel is a special transform direction variabel by
        // getting the transform of the player and using the TransformDirection() method to change it accordingly
        moveDirection = transform.TransformDirection(moveDirection);
        // multiple this by the walkSpeed so the player can move at that set speed
        moveDirection *= walkSpeed;
        // is the move direciton isn't 0 (meaning there was input) and the extra fire button key wasnt pressed then:
        if (moveDirection != Vector3.zero && !Input.GetButton("Fire2"))
        {
            Walk(); // play the walk animation
        }
        // but is the extra fire button was pressed, then move at a faster speed
        else if (moveDirection != Vector3.zero && Input.GetButton("Fire2"))
        {
            Run(); // play the run animation
        }
        // and after all that if the move direction is null, then keep the player idle
        else if (moveDirection == Vector3.zero)
        {
            Idle(); // play the idle animations and do nothing
        }
        
        // multipley the move direction by the move speed (which would have chagnegd accroding to the input and move functions above)
        moveDirection *= moveSpeed;

        // move the controller in that direction using the Move() method times time.deltatime (just so the movement is smooth)
        controller.Move(moveDirection * Time.deltaTime);
    }

    /// <summary>
    /// Make the player play an idle animation
    /// </summary>
    private void Idle()
    {
        // set the Speed float in the animator to 0, stating that it should
        // trasision from what ever it was to idle anim by .05 * time.deltatime (i.e. make the transition to idle smooth)
        anim.SetFloat("Speed", 0, 0.05f, Time.deltaTime);
    }

    /// <summary>
    /// Make the player play the run antimations and make the player faster
    /// </summary>
    private void Run()
    {
        // make the current moveSpeed equal to the set runSpeed
        moveSpeed = runSpeed;
        // set the Speed float in the animator to 1, stating that it should
        // trasision from what ever it was to the run anim by .05 * time.deltatime (i.e. make the transition to runing smooth)
        anim.SetFloat("Speed", 1, 0.05f, Time.deltaTime);
    }

    /// <summary>
    /// make the player play the walk animations and set the speed to the walk speed
    /// </summary>
    private void Walk()
    {
        // make the current moveSpeed equal to the set walkSpeed
        moveSpeed = walkSpeed;
        // set the Speed float in the animator to .5, stating that it should
        // trasision from what ever it was to the walk anim by .05 * time.deltatime (i.e. make the transition to walking smooth)
        anim.SetFloat("Speed", 0.5f, 0.05f, Time.deltaTime);
    }

    /// <summary>
    /// handle the rotation of the player based on horizontal input
    /// </summary>
    private void Rotatee()
    {
        // if the horizontal input (which is a 1 if right pressed and vice versa) is above 0 (right) then:
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            // rotate the trasform compoenet of this player along the y axis at the turn speed in the right direction
            transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
        }
        // if the horizontal input (which is a 1 if right pressed and vice versa) is below 0 (right) then:
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            // rotate the trasform compoenet of this player along the y axis at the turn speed in the left direction
            transform.Rotate(Vector3.up, -turnSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Play the attack animations and sound effects
    /// </summary>
    /// <returns>nothing</returns>
    IEnumerator Attack()
    {
        // set attacking to true so that the rest of the script know that
        attacking = true;
        // trigger the attak animation
        anim.SetTrigger("Attack");
        // randomise the sound of the player attack sound
        RandomiseAttackSound();
        // play the audio source which has the attacksound on it
        attackSound.Play();
        // pause for 1 second so that the player cant attack continously
        yield return new WaitForSeconds(1);
        // make attacking equal false so the player doesnt keep hruting the enemy elsewhere in this script
        attacking = false;
    }

    /// <summary>
    /// When the player collides with something, this methos trigger and is
    /// primarily used to deak damage to the enemies.
    /// </summary>
    /// <param name="collision">this is the information about he collision</param>
    private void OnCollisionEnter(Collision collision)
    {
        // if the player is attacking
        if (attacking == true)
        {
            // if the tag of the gameobject of the collsion is SpiderTag then:
            if (collision.gameObject.tag == "SpiderTag")
            {
                // get the spidercontrolelr script from the gameobject and - 
                spiderController = collision.gameObject.GetComponent<SpiderController>();
                // - call the TakeDamage() function on it which will take damage from the enemy
                spiderController.TakeDamage();
            }
            // Repeat the same process as above but for the WizardTag and wizardController
            if (collision.gameObject.tag == "WizardTag")
            {
                wizardController = collision.gameObject.GetComponent<WizardController>();
                wizardController.TakeDamage();
            }
            // Repeat the same process as above but for the GuardTag and guardController
            if (collision.gameObject.tag == "GuardTag")
            {
                guardController = collision.gameObject.GetComponent<GuardController>();
                guardController.TakeDamage();
            }
            // Repeat the same process as above but for the BossTag and bossController
            if (collision.gameObject.tag == "BossTag")
            {
                bossController = collision.gameObject.GetComponent<BossController>();
                bossController.TakeDamage();
            }
        }
    }

    /// <summary>
    /// this function only play when the collider of the player eneters a trgger collider of another game object
    /// </summary>
    /// <param name="other">this stores the info of the collisions collider</param>
    private void OnTriggerEnter(Collider other)
    {
        // if the tag of the gameobject of the collider collided with is RockTag then:
        if (other.gameObject.tag == "RockTag")
        {
            // play the hit by rock sound from the hitByRockSound audiosource
            hitByRockSound.Play();
            // decrease the player health by the speical attack damage (currently 10)
            playerHealth -= specialAttackDamage;
        }
        // if the tag of the gameobject of the collider collided with is FloorFireTag then:
        if (other.gameObject.tag == "FloorFireTag")
        {
            // decrease the player health by the speical attack damage (currently 10)
            playerHealth -= specialAttackDamage;
        }
    }

    /// <summary>
    /// this adds a key to the toal amount the player has
    /// </summary>
    public void addKey()
    {
        // inscrease the keysCollected hence signaling the player has one more key
        keysCollected++;
    }

    /// <summary>
    /// This void is used to randomise the pitch and volume of the player attack sound
    /// </summary>
    void RandomiseAttackSound()
    {
        // set the pitch of the attack sound audiosource to between .75 and 1.25
        attackSound.pitch = Random.Range(0.75f, 1.25f);
        // set the volume of the attack sound audiosource to between .75 and 1
        attackSound.volume = Random.Range(0.75f, 1f);
    }

    /// <summary>
    /// This is the Lose Game fucntion called when the player runs fully out of health
    /// </summary>
    /// <returns>nothing</returns>
    IEnumerator LoseGame()
    {
        // Play the Die animation
        anim.Play("Die");
        // wait for the first animtion playing in the animator (i.e. the death animation) to finich playing
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        // Play the lose sound audiosource
        loseSound.Play();
        // set the lose menu screen active to make it appear on the screen
        loseScreen.SetActive(true);
        // set the isPlaying variable in the gamemanger script to false to signal
        // that the game is not active and so the input can be stopped
        gameManager.isPlaying = false;
        // set the timescale of the time to 0 to stop any action that require a chaneg in time (such as animtions)
        Time.timeScale = 0f;
    }
}
