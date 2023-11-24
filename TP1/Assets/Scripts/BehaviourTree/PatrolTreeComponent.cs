using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PatrolComponent : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float detectionRange = 5;
    [SerializeField] List<Transform> targets = new List<Transform>();
    [SerializeField] float waitTime = 3;

    public bool isJumping = false;

    NavMeshAgent agent;
    Node root;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        SetupTree();
    }

    // Update is called once per frame
    void Update()
    {
        root.Evaluate();
    }

    private void SetupTree()
    {
        Node l1 = new IsWithinRange(transform, target, detectionRange);
        Node l2 = new GoToTarget(target, agent);


        Node sequence1 = new Sequence(new List<Node>() { l1, l2 });

        Node l4 = new JumpRequired(transform, target, 10f);
        Node l5 = new JumpOnTarget(target, agent, transform.GetComponent<Rigidbody>(), 7f, 1.5f);

        Node sequence2 = new Sequence(new List<Node>() { l4, l5 });

        Node l3 = new PatrolTask(targets, agent, waitTime);
        Node sel1 = new Selector(new List<Node>() { sequence2, sequence1, l3 });
        root = sel1;



    }
}