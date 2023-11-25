using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class FollowComponent : MonoBehaviour
{
    [SerializeField] Transform destination;
    [SerializeField] GameObject portal;
    Animator animator;

    NavMeshAgent agent;

    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = destination.position; // set une fois pour les destinations statiques 
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!agent.enabled)
            return;

        if (Vector3.Distance(portal.transform.position, transform.position) <= 2)
        {
            animator.SetTrigger("Jump");
        }
        else
        {
            animator.SetBool("IsWalking", true);
        }

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            animator.SetBool("IsWalking", false);
            animator.SetBool("Arrived", true);
        }

        agent.destination = destination.position;
    }
}
