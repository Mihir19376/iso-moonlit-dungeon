using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// this script controls the guard enemy and all the attributes associated to it
/// </summary>
public class GuardController : MonoBehaviour
{
    // This is the Health of the Guard 
    // It will decrease as the player damages it
    public int guardHealth;
    // This is a reference to the NavMesh Agent component attachted to the object attached to this script 
    // I pass instructions of where to head to this component and it uses them to move the GameObject to there.
    NavMeshAgent guardNavMeshAgent;
    // A refernce to the Animator component
    // I will pass instructions to this and it will animate the character as such
    private Animator guardAnim;
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
    // a boolean that states if this enemy is dead. This is used so that the enemy can't damage the player whens its death anim is playing
    private bool dead;
    // a refrence to the game manager script
    GameManager gameManager;
    // this is a refernce to a rock prefab that the enemy can use to throw at and attck the player
    public GameObject rock;
    // a refrecne to the speed of the rock
    public float rockSpeed = 10;
    // a boolean stating if the enemy can or cant attack. This will allow or not allow the enemy to attack the player 
    private bool canAttack;
    // this is a reference to the green explosion which is used to show the enemies death.
    public GameObject greenExplosionFX;
    // this is the max distance this enemy has to be to the player to move towards it
    private int movingDistance = 3;

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
        guardHealth = 10;
        // get the player controller compoenet (script) from the player game object and set it to "playerControlelr" 
        playerController = GameObject.FindGameObjectWithTag("PlayerTag").GetComponent<PlayerController>();
        // get the animator controller from the game object that this script is attached to and set it to "guardAnim"
        guardAnim = GetComponent<Animator>();
        // in the same method as above ˆ retrive the NavMesh Agent and set it to "guardNavMeshAgent"
        guardNavMeshAgent = GetComponent<NavMeshAgent>();
        // when it is instaniated, the enemy should be able to attack.
        canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        // throws rocks randomly at the player to damage them
        StartCoroutine(Throw());
        // make the enemy face the player
        LookAt();
        // check if the health variable is equivilant or less than 0, and if so carry out the following:
        if (guardHealth <= 0 && dead == false)
        {
            // kill the enemy (by playing all the required animations and stuff)
            StartCoroutine(dealPlayerDamage());
            // tell the game manager there is one less of this type of enemy in
            // the level by reducing the guardEnemiesLeft variable by one
            gameManager.guardEnemiesLeft -= 1;
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
            if (distanceToPlayer <= movingDistance)
            {
                // re-instialising the nav meash agent by setting its isStopped valeu to false
                guardNavMeshAgent.isStopped = false;
                // say that the enemy is no longer idle and can move around
                idle = false;
                // moves towards and follow the player and play the nececary animations
                FollowPlayer();
            }
            // if the enemy is too far from the player then make it idle by:
            else
            {
                // disabling the nav mesh agent by setting its isStopped value to true
                guardNavMeshAgent.isStopped = true;
                // stateing that the enemy is infact idle
                idle = true;
                // set the animations speed float to 0 which promts it to
                // transition to its idle anim (the .05f and time.deltatime are to just make this transition smooth)
                guardAnim.SetFloat("Speed", 0, 0.05f, Time.deltaTime);
            }

            // if the navmesh agent of the enemy has a path and isnt idle and - 
            if (!guardNavMeshAgent.pathPending && idle == false)
            {
                // - the enemies reaining distance to he player is smaller than or equal to the stopping distance and - 
                if (guardNavMeshAgent.remainingDistance <= guardNavMeshAgent.stoppingDistance)
                {
                    // - it doesnt have any path left or its velocity is null then - 
                    if (!guardNavMeshAgent.hasPath || guardNavMeshAgent.velocity.sqrMagnitude == 0f)
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
        guardNavMeshAgent.SetDestination(playerTransform.position);
        // if the oldposition of the enemy isnt equal to its current position
        // then it has moved and so sets the animators speed int to 0 triggering its walking animation
        if (oldPos != transform.position)
        {
            guardAnim.SetFloat("Speed", 1, 0.05f, Time.deltaTime);
        }
        // but if the position was the same, then this means the enemy hasn't
        // moved and so the animations is set to 0 which is a idle anim
        else
        {
            guardAnim.SetFloat("Speed", 0, 0.05f, Time.deltaTime);
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
        guardAnim.SetTrigger("Attack");
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
        guardHealth -= 1;
    }

    /// <summary>
    /// This will deal the last blow of damge from the play to the enemy and kill it
    /// </summary>
    /// <returns>nothing</returns>
    IEnumerator dealPlayerDamage()
    {
        Debug.Log("This Enemy has Died"); // print that this enemy has died
        guardAnim.Play("Die"); // play the die animation
        // wait for the animation to finish
        yield return new WaitForSeconds(guardAnim.GetCurrentAnimatorStateInfo(0).length);
        playerController.addKey(); // then hand the player a key
        // spawn a particle explosion where this enemy is
        Instantiate(greenExplosionFX, transform.position, Quaternion.identity);
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
    /// this function will throw a rock at the player at random
    /// </summary>
    /// <returns>nothing</returns>
    IEnumerator Throw()
    {
        // the 1500 isnt set as a variable becasuse there is no need as it is used only once.
        // Making it a viable wouldnt make any easier to chaneg than this and would need more code
        // if a number between 0 and 1500 is equal to 5 and if the navmesh is stopped (as is the enemy is idle) then:
        if (Random.Range(0, 1500) == 5 && guardNavMeshAgent.isStopped == true)
        {
            // play the throw animation
            guardAnim.SetTrigger("Throw");
            // wait until the anim is done playing
            yield return new WaitForSeconds(guardAnim.GetCurrentAnimatorStateInfo(0).length);
            // then get the position of this enemy with a slighty different adjusted y value so it can hit the plyer
            Vector3 rockPos = new Vector3(transform.position.x, 1, transform.position.z);
            // instantiate a rock at this positon above and at the rotaion of
            // this enemy and store this in a variabel called bullet
            // (it was the closet work I could think of that meant the same thing)
            var bullet = Instantiate(rock, rockPos, transform.rotation);
            // get the rigibody component from this supposed bullet and change
            // its velocity to move forward at the rock speed
            bullet.GetComponent<Rigidbody>().velocity = transform.forward * rockSpeed;
        }
    }
}
