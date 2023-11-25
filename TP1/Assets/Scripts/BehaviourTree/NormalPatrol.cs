using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NormalPatrol : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float detectionRange = 5;
    [SerializeField] List<Transform> targets = new List<Transform>();
    [SerializeField] float waitTime = 3;
    [SerializeField] GameObject portal;
    Animator animator;

    public bool isJumping = false;

    NavMeshAgent agent;
    Node root;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        SetupTree();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(portal.transform.position, transform.position) <= 2)
        {
            animator.SetTrigger("Jump");
        }

        root.Evaluate();
    }

    private void SetupTree()
    {
        Node l1 = new IsWithinRange(transform, target, detectionRange);
        Node l2 = new GoToTarget(target, agent);


        Node sequence1 = new Sequence(new List<Node>() { l1, l2 });


        Node l3 = new PatrolTask(targets, agent, waitTime);
        Node sel1 = new Selector(new List<Node>() { sequence1, l3 });
        root = sel1;



    }
}