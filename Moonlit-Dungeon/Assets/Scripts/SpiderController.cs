using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpiderController : MonoBehaviour
{
    NavMeshAgent spiderNavMeshAgent;
    private Animator spiderAnim;

    public Transform playerTransform;
    private Vector3 oldPos;

    // Start is called before the first frame update
    void Start()
    {
        spiderAnim = GetComponent<Animator>();
        spiderNavMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        spiderNavMeshAgent.SetDestination(playerTransform.position);
        if (oldPos != transform.position)
        {
            spiderAnim.SetFloat("Speed", 1);
        }
        else
        {
            spiderAnim.SetFloat("Speed", 0);
        }
        oldPos = transform.position;
    }
}
