using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpiderController : MonoBehaviour
{
    public int spiderHealth;

    NavMeshAgent spiderNavMeshAgent;
    private Animator spiderAnim;

    public Transform playerTransform;
    private Vector3 oldPos;

    PlayerController playerController;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        spiderHealth = 10;
        playerController = player.GetComponent<PlayerController>();
        spiderAnim = GetComponent<Animator>();
        spiderNavMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
        if (!spiderNavMeshAgent.pathPending)
        {
            if (spiderNavMeshAgent.remainingDistance <= spiderNavMeshAgent.stoppingDistance)
            {
                if (!spiderNavMeshAgent.hasPath || spiderNavMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    Attack();
                }
            }
        }

    }

    private void FollowPlayer()
    {
        spiderNavMeshAgent.SetDestination(playerTransform.position);
        if (oldPos != transform.position)
        {
            spiderAnim.SetFloat("Speed", 1, 0.05f, Time.deltaTime);
        }
        else
        {
            spiderAnim.SetFloat("Speed", 0, 0.05f, Time.deltaTime);
        }
        oldPos = transform.position;
    }

    private void Attack()
    {
        spiderAnim.SetTrigger("Attack");
        playerController.playerHealth -= 1;
    }
}
