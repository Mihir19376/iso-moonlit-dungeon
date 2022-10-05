using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossController : MonoBehaviour
{
    // This is the Health of the Spider
    // It will decrease as the player damages it
    public int bossHealth;
    // This is a reference to the NavMesh Agent component attachted to the object attached to this script 
    // I pass instructions of where to head to this component and it uses them to move the GameObject to there.
    NavMeshAgent bossNavMeshAgent;
    // A refernce to the Animator component
    // I will pass instructions to this and it will animate the character as such
    private Animator bossAnim;
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

    // Start is called before the first frame update
    // Anything set here is set once at the start of the game
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManagerTag").GetComponent<GameManager>();
        dead = false;
        player = GameObject.FindGameObjectWithTag("PlayerTag");
        playerTransform = GameObject.FindGameObjectWithTag("PlayerTag").GetComponent<Transform>();
        // Set the health of the spider to 10
        bossHealth = 50;
        // get the player controller compoenet (script) from the player game object and set it to "playerControlelr" 
        playerController = GameObject.FindGameObjectWithTag("PlayerTag").GetComponent<PlayerController>();
        // get the animator controller from the game object that this script is attached to and set it to "spiderAnim"
        bossAnim = GetComponent<Animator>();
        // in the same method as above ? retrive the NavMesh Agent and set it to "spiderNavMeshAgent"
        bossNavMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        LookAt();
        // check if the health variable is equivilant or less than 0, and if so carry out the following:
        if (bossHealth <= 0 && dead == false)
        {
            playerController.hasTreasure = true;
            StartCoroutine(dealPlayerDamage());
            gameManager.bossEnemiesLeft -= 1;
            dead = true;
        }
        else
        {
            distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer <= 5)
            {
                bossNavMeshAgent.isStopped = false;
                idle = false;
                FollowPlayer();
            }
            else
            {
                bossNavMeshAgent.isStopped = true;
                idle = true;
                bossAnim.SetFloat("Speed", 0, 0.05f, Time.deltaTime);
            }


            if (!bossNavMeshAgent.pathPending && idle == false)
            {
                if (bossNavMeshAgent.remainingDistance <= bossNavMeshAgent.stoppingDistance)
                {
                    if (!bossNavMeshAgent.hasPath || bossNavMeshAgent.velocity.sqrMagnitude == 0f)
                    {
                        StartCoroutine(Attack());
                        //Attack();
                    }
                }
            }
        }

    }

    private void FollowPlayer()
    {
        bossNavMeshAgent.SetDestination(playerTransform.position);
        if (oldPos != transform.position)
        {
            bossAnim.SetFloat("Speed", 1, 0.05f, Time.deltaTime);
        }
        else
        {
            bossAnim.SetFloat("Speed", 0, 0.05f, Time.deltaTime);
        }
        oldPos = transform.position;
    }

    IEnumerator Attack()
    {
        bossAnim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f);
        playerController.playerHealth -= 1;
    }

    public void TakeDamage()
    {
        bossHealth -= 1;
    }

    IEnumerator dealPlayerDamage()
    {
        gameManager.bossDefeated = true;
        Debug.Log("This Enemy has Died");
        bossAnim.Play("Die");
        yield return new WaitForSeconds(bossAnim.GetCurrentAnimatorStateInfo(0).length);
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
}
