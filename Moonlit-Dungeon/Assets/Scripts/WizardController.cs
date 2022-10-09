using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WizardController : MonoBehaviour
{
    // This is the Health of the Spider
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

    private bool dead;

    GameManager gameManager;

    public AudioSource wizardAudioSource;

    // Start is called before the first frame update
    // Anything set here is set once at the start of the game
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManagerTag").GetComponent<GameManager>();
        dead = false;
        player = GameObject.FindGameObjectWithTag("PlayerTag");
        playerTransform = GameObject.FindGameObjectWithTag("PlayerTag").GetComponent<Transform>();
        // Set the health of the spider to 10
        wizardHealth = 10;
        // get the player controller compoenet (script) from the player game object and set it to "playerControlelr" 
        playerController = GameObject.FindGameObjectWithTag("PlayerTag").GetComponent<PlayerController>();
        // get the animator controller from the game object that this script is attached to and set it to "spiderAnim"
        wizardAnim = GetComponent<Animator>();
        // in the same method as above ˆ retrive the NavMesh Agent and set it to "spiderNavMeshAgent"
        wizardNavMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(RandomMove());
        LookAt();
        // check if the health variable is equivilant or less than 0, and if so carry out the following:
        if (wizardHealth <= 0 && dead == false)
        {
            //// print that the enemy has died in the debug termianl in Unity
            //Debug.Log("This Enemy has Died");
            //// carry out the addKey function in the player controller. 
            //// This will increase the number of keys that the player has obtained by one.
            //playerController.addKey();
            //// tell the animator compoenet to play the "Die" animation
            //spiderAnim.Play("Die");
            //gameObject.SetActive(false);
            StartCoroutine(dealPlayerDamage());
            gameManager.wizardEnemiesLeft -= 1;
            dead = true;
        }
        else
        {
            distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer <= 3)
            {
                wizardNavMeshAgent.isStopped = false;
                idle = false;
                FollowPlayer();
            }
            else
            {
                wizardNavMeshAgent.isStopped = true;
                idle = true;
                wizardAnim.SetFloat("Speed", 0, 0.05f, Time.deltaTime);
            }


            if (!wizardNavMeshAgent.pathPending && idle == false)
            {
                if (wizardNavMeshAgent.remainingDistance <= wizardNavMeshAgent.stoppingDistance)
                {
                    if (!wizardNavMeshAgent.hasPath || wizardNavMeshAgent.velocity.sqrMagnitude == 0f)
                    {
                        if (gameManager.isPlaying)
                        {
                            StartCoroutine(Attack());
                        }
                        //Attack();
                    }
                }
            }
        }

    }

    private void FollowPlayer()
    {
        wizardNavMeshAgent.SetDestination(playerTransform.position);
        if (oldPos != transform.position)
        {
            wizardAnim.SetFloat("Speed", 1, 0.05f, Time.deltaTime);
        }
        else
        {
            wizardAnim.SetFloat("Speed", 0, 0.05f, Time.deltaTime);
        }
        oldPos = transform.position;
    }

    IEnumerator Attack()
    {
        wizardAnim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f);
        playerController.playerHealth -= 1;
    }

    public void TakeDamage()
    {
        wizardHealth -= 1;
    }

    IEnumerator dealPlayerDamage()
    {
        Debug.Log("This Enemy has Died");
        wizardAnim.Play("Die");
        yield return new WaitForSeconds(wizardAnim.GetCurrentAnimatorStateInfo(0).length);
        playerController.addKey();
        //gameObject.SetActive(false);
        Destroy(gameObject);
    }

    void LookAt()
    {
        Vector3 direction = (playerTransform.position - transform.position);
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 20);
    }


    IEnumerator RandomMove()
    {
        if (Random.Range(0, 1500) == 5 && wizardNavMeshAgent.isStopped == true)
        {
            wizardAudioSource.Play();
            transform.position = new Vector3(gameManager.generateRandomPointOnNavMesh(), 0, gameManager.generateRandomPointOnNavMesh());
        }

        yield return new WaitForSeconds(1);
    }
}
