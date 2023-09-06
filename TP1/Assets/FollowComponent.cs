using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowComponent : MonoBehaviour
{
    [SerializeField] Transform destination;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = destination.position; // set une fois pour les destinations statiques 
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = destination.position;

    }
}

