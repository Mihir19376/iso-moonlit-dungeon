using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WizardController : MonoBehaviour
{
    // This is the Health of the Wizard
    // It will decrease as the player damages it
    public int wizardHealth;
    // This is a reference to the NavMesh Agent component attachted to the object attached to this script 
    // I pass instructions of where to head to this component and it uses them to move the GameObject to there.
    NavMeshAgent wizardNavMeshAgent;
    // A refernce to the Animator component
    // I will pass instructions to this and it will animate the character as such
    private Animator wizardAnim;
    // This is a refernce to the Transform Component of the player that this enemy is trying to deal damage to
    // The enemy will use this variable to find the position of the player and feed tha into the agent whcih moves the enemy there
    public Transform playerTransform;
    // This stores the position of the enemy before it moved. It is used to trigger the idle and move animation
    private Vector3 oldPos;
    // This is a refernce to the player script named PlayerController. From this reference, this script can acces its variables and voids
    PlayerController playerController;
    // a reference to the Game Object of the player that is in the scene
    public GameObject player;
    // a variabel that stores the distance that this enemy is to the player
    private float distanceToPlayer;
    // A boolean that states if this enemy is idle or not, and will play the according animations
    private bool idle;
    // a boolean that states if this enemy is dead. This is used so that the enemy can't damage the player whens its death anim
    private bool dead;
    // a refrence to the game manager script
    GameManager gameManager;
    // this is the audio source that stores the wizards woosh sound 
    public AudioSource wizardAudioSource;
    // a boolean stating if the enemy can or cant attack. This will allow or not allow the enemy to attack the player when it needs to
    private bool canAttack;
    // this is a reference to the purple explosion which is used to show the enemies death.
    public GameObject purpleExplosionFX;
    // this is rhe smoke particle effect that is used to hide the woazrds disaperence
    public GameObject teleportFX;

    // Start is called before the first frame update
    // Anything set here is set once at the start of the game
    void Start()
    {
        // get the gamemanger script component fromt the object with the GameManagerTag in the heirachy
        gameManager = GameObject.FindGameObjectWithTag("GameManagerTag").GetComponent<GameManager>();
        // obviously the enemy is not dead when it is initailly instantiated, which is why its set to not that
        dead = false;
        // find the game obejct with the PlayerTag in the heriachy
        player = GameObject.FindGameObjectWithTag("PlayerTag");
        // get the trasnform components from the game object that has the player tag on it
        playerTransform = GameObject.FindGameObjectWithTag("PlayerTag").GetComponent<Transform>();
        // Set the health of the guard to 10
        wizardHealth = 10;
        // get the player controller compoenet (script) from the player game object and set it to "playerControlelr" 
        playerController = GameObject.FindGameObjectWithTag("PlayerTag").GetComponent<PlayerController>();
        // get the animator controller from the game object that this script is attached to and set it to "wizardAnim"
        wizardAnim = GetComponent<Animator>();
        // in the same method as above ˆ retrive the NavMesh Agent and set it to "wizardNavMeshAgent"
        wizardNavMeshAgent = GetComponent<NavMeshAgent>();
        // when it is instaniated, the enemy should be able to attack.
        canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        // teleport and move this enemy to random places among the floor
        StartCoroutine(RandomMove());
        // make the enemy face the player
        LookAt();
        // check if the health variable is equivilant or less than 0, and if so carry out the following:
        if (wizardHealth <= 0 && dead == false)
        {
            // kill the enemy (by playing all the required animations and stuff)
            StartCoroutine(EnemyDie());
            // tell the game manager there is one less of this type of enemy in
            // the level by reducing the wizardEnemiesLeft variable by one
            gameManager.wizardEnemiesLeft -= 1;
            // state that the enemy is dead so that this if statement cant run over and over
            dead = true;
        }
        // but if the enemy isnt dead then:
        else
        {
            // Calculate the distance to the player by using the Distance
            // funcition with the players position and this enemies position as an input
            distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            // if the distance to the player is less than the max distance then move towards the player by:
            if (distanceToPlayer <= 3)
            {
                // re-instialising the nav mesh agent by setting its isStopped value to false
                wizardNavMeshAgent.isStopped = false;
                // say that the enemy is no longer idle and can move around
                idle = false;
                // moves towards and follow the player and play the nececary animations
                FollowPlayer();
            }
            // if the enemy is too far from the player then make it idle by:
            else
            {
                // disabling the nav mesh agent by setting its isStopped value to true
                wizardNavMeshAgent.isStopped = true;
                // stateing that the enemy is infact idle
                idle = true;
                // set the animations speed float to 0 which promts it to
                // transition to its idle anim (the .05f and time.deltatime are to just make this transition smooth)
                wizardAnim.SetFloat("Speed", 0, 0.05f, Time.deltaTime);
            }

            // if the navmesh agent of the enemy has a path and isnt idle and - 
            if (!wizardNavMeshAgent.pathPending && idle == false)
            {
                // - the enemies reaining distance to he player is smaller than or equal to the stopping distance and - 
                if (wizardNavMeshAgent.remainingDistance <= wizardNavMeshAgent.stoppingDistance)
                {
                    // - it doesnt have any path left or its velocity is null then - 
                    if (!wizardNavMeshAgent.hasPath || wizardNavMeshAgent.velocity.sqrMagnitude == 0f)
                    {
                        // if isPlayer from gameManager equals true (meaning the game is active and not in a menu) - 
                        if (gameManager.isPlaying)
                        {
                            // - and if the enemy can attack then - 
                            if (canAttack)
                            {
                                // - start the attack coroutine where the enemy
                                // deals demage to player and plays the appropriate animations
                                StartCoroutine(Attack());
                            }
                            
                        }
                    }
                }
            }
        }

    }

    /// <summary>
    /// This functions will make the enemy follow the
    /// player when it is allowed to and play the appropriate animations when doing so
    /// </summary>
    private void FollowPlayer()
    {
        // set the desitination of the nav mesh (where the enemy must head) to
        // be the position of the player at any givern time
        wizardNavMeshAgent.SetDestination(playerTransform.position);
        // if the oldposition of the enemy isnt equal to its current position
        // then it has moved and so sets the animators speed int to 0 triggering its walking animation
        if (oldPos != transform.position)
        {
            wizardAnim.SetFloat("Speed", 1, 0.05f, Time.deltaTime);
        }
        // but if the position was the same, then this means the enemy hasn't
        // moved and so the animations is set to 0 which is a idel anim
        else
        {
            wizardAnim.SetFloat("Speed", 0, 0.05f, Time.deltaTime);
        }
        // now set the new oldPos to the now current position
        oldPos = transform.position;
    }

    /// <summary>
    /// This will make the enemy attack the player
    /// </summary>
    /// <returns>doesnt return anything</returns>
    IEnumerator Attack()
    {
        // trigger the attack animation
        wizardAnim.SetTrigger("Attack");
        // make it so that the enemy cant attack for 0.5s so that its doesnt continously lower playe health. 
        canAttack = false;
        yield return new WaitForSeconds(0.5f);
        canAttack = true;
        // then low the player health by 1
        playerController.playerHealth -= 1;
    }

    /// <summary>
    /// damage the enemy
    /// </summary>
    public void TakeDamage()
    {
        // reduce the health by an amount (1)
        wizardHealth -= 1;
    }

    /// <summary>
    /// This will deal the last blow of damge from the play to the enemy and kill it
    /// </summary>
    /// <returns>nothing</returns>
    IEnumerator EnemyDie()
    {
        Debug.Log("This Enemy has Died"); // print that this enemy has died
        wizardAnim.Play("Die"); // play the die animation
        // wait for the animation to finish
        yield return new WaitForSeconds(wizardAnim.GetCurrentAnimatorStateInfo(0).length);
        playerController.addKey(); // then hand the player a key
        // spawn a particle explosion where this enemy is
        Instantiate(purpleExplosionFX, transform.position, Quaternion.identity);
        // and destroy this enemy from the hierachy
        Destroy(gameObject);
    }

    /// <summary>
    /// Make the enemy look at the player constanlty
    /// </summary>
    void LookAt()
    {
        // set positions direction to look at the postion of the player minus the position of this enemy
        Vector3 direction = (playerTransform.position - transform.position);
        // then make the direction/rotation to look at with the direction.x and directoin.z as positions to look at
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        // change the rotation of the player to look at the direction the player
        // is from the enemy. and change this rotation at a speed so it doesnt snap in and out of it
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 20);
    }

    /// <summary>
    /// Move this wizard to random spots among the arena
    /// </summary>
    /// <returns>nothing</returns>
    IEnumerator RandomMove()
    {
        // the 1500 isnt set as a variable becasuse there is no need as it is used only once.
        // Making it a viable wouldnt make any easier to chaneg than this and would need more code
        // if a number between 0 and 1500 is equal to 5 and if the navmesh is stopped (as is the enemy is idle) then:
        if (Random.Range(0, 1500) == 5 && wizardNavMeshAgent.isStopped == true)
        {
            // spawn the teleport particle effects at the current position of this wizard and at the regular rotation
            Instantiate(teleportFX, transform.position, Quaternion.identity);
            // play the wind woosh sound from the wizars only audio source
            wizardAudioSource.Play();
            // make the new position of this a random one among the palying field. This is done by setting the position to be a randomly genrated one from the gamemanagers script 
            transform.position = new Vector3(gameManager.generateRandomPointOnNavMesh(), 0, gameManager.generateRandomPointOnNavMesh());
        }
        // pause this for 1 seconds so that it cant happen instantanously again
        yield return new WaitForSeconds(1);
    }
}
