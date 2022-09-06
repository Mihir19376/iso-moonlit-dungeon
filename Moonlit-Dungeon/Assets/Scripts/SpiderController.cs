using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpiderController : MonoBehaviour
{
    NavMeshAgent spiderNavMeshAgent;

    public Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        spiderNavMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        spiderNavMeshAgent.SetDestination(playerTransform.position);
    }
}
