using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum NodeState { Running, Success, Failure }

public abstract class Node
{
    protected NodeState state;
    public Node parent { get; set; }
    protected List<Node> children = new();
    Dictionary<string, object> data = new();

    public Node()
    {
        parent = null;
        state = NodeState.Running;
    }

    public Node(List<Node> pChildren)
    {
        parent = null;
        state = NodeState.Running;
        foreach (Node child in pChildren)
            Attach(child);
    }

    protected void Attach(Node n)
    {
        children.Add(n);
        n.parent = this;
    }

    public abstract NodeState Evaluate();

    public void SetDate(string key, object value)
    {
        data[key] = value;
    }

    public object GetDate(string key)
    {
        if (data.TryGetValue(key, out object value))
            return value;
        if (parent != null)
            return parent.GetDate(key);

        return null;
    }

    public bool TryRemoveData(string key)
    {
        if (data.Remove(key))
            return true;
        if (parent != null)
            return parent.TryRemoveData(key);
        return false;
    }
}

public class Sequence : Node
{
    public Sequence(List<Node> n) : base(n) { }
    public override NodeState Evaluate()
    {
        foreach (Node n in this.children)
        {
            state = n.Evaluate();
            if (state == NodeState.Running)
                return NodeState.Running;
            if (state == NodeState.Failure)
                return NodeState.Failure;
        }
        return NodeState.Success;
    }
}

public class Selector : Node
{
    public Selector(List<Node> n) : base(n) { }

    public override NodeState Evaluate()
    {
        foreach (Node n in this.children)
        {
            state = n.Evaluate();
            if (state != NodeState.Failure)
                return state;
        }

        state = NodeState.Failure;
        return NodeState.Failure;
    }
}

public class Inverter : Node
{
    public Inverter(List<Node> n) : base()
    {
        if (n.Count != 1)
            throw new System.ArgumentException();
    }

    public override NodeState Evaluate()
    {
        NodeState childState = children[0].Evaluate();

        if (childState == NodeState.Success)
            state = NodeState.Failure;
        else if (childState == NodeState.Failure)
            return NodeState.Success;
        else
            state = NodeState.Running;

        return state;
    }
}

public class IsWithinRange : Node
{
    Transform self, target;
    float detectRange;

    public IsWithinRange(Transform self, Transform target, float detectRange) : base()
    {
        this.self = self;
        this.target = target;
        this.detectRange = detectRange;
    }
    public override NodeState Evaluate()
    {
        state = NodeState.Failure;
        if (Vector3.Distance(self.position, target.position) <= detectRange && self.position.y + .2f >= target.position.y)
            state = NodeState.Success;

        return state;
    }
}

public class GoToTarget : Node
{
    Transform target;
    NavMeshAgent agent;

    public GoToTarget(Transform target, NavMeshAgent agent) : base()
    {
        this.target = target;
        this.agent = agent;
    }

    public override NodeState Evaluate()
    {
        if (!agent.enabled)
            return NodeState.Failure;

        agent.destination = target.position;

        if (agent.remainingDistance <= agent.stoppingDistance)
            state = NodeState.Success;
        if (agent.SetDestination(agent.destination))
            state = NodeState.Running;
        else
            state = NodeState.Failure;

        return state;
    }
}

public class JumpRequired : Node
{
    Transform self, target;
    float jumpRange;

    float jumpReq = .1f;

    public JumpRequired(Transform self, Transform target, float jumpRange = 10f) : base()
    {
        this.self = self;
        this.target = target;
        this.jumpRange = jumpRange;
    }
    public override NodeState Evaluate()
    {
        state = NodeState.Failure;

        if (target.position.y > self.position.y + jumpReq && target.position.y < self.position.y + jumpRange)
            state = NodeState.Success;
        else if (!self.GetComponent<PatrolComponent>().isJumping)
        {
            self.GetComponent<NavMeshAgent>().enabled = true;
            return state;
        }

        return state;
    }
}

public class JumpOnTarget : Node
{
    Transform target;
    NavMeshAgent agent;
    Rigidbody rigidbody;

    float jumpHeight;
    float jumpDuration;
    float jumpTime = .8f;

    float elapsedJumpTime = 0;
    bool isWaiting = false;

    public bool IsGrounded()
    {
        Ray ray = new Ray(agent.transform.position, Vector3.down);

        if (Physics.Raycast(ray, .2f))
        {
            return true;
        }

        return false;
    }

    public JumpOnTarget(Transform target, NavMeshAgent agent, Rigidbody rigidbody, float jumpHeight = 6f, float jumpDuration = 1.5f) : base()
    {
        this.target = target;
        this.agent = agent;
        this.rigidbody = rigidbody;
        this.jumpHeight = jumpHeight;
        this.jumpDuration = jumpDuration;
    }

    public override NodeState Evaluate()
    {
        state = NodeState.Running;

        if (isWaiting)
        {
            elapsedJumpTime += Time.deltaTime;

            if (elapsedJumpTime >= .55f)
            {
                agent.enabled = true;
                agent.transform.GetComponent<PatrolComponent>().isJumping = false;
                agent.SetDestination(target.position);

            }

            if (elapsedJumpTime >= jumpDuration)
            {
                isWaiting = false;
                agent.transform.GetComponent<PatrolComponent>().isJumping = false;
                elapsedJumpTime = 0;

                rigidbody.useGravity = false;
                rigidbody.isKinematic = true;
                agent.enabled = true;


                if (target.position.y < agent.transform.position.y + .2f)
                    return NodeState.Success;
                else 
                    return NodeState.Failure;
            }
        }
        else
        {
            Jump();

            return state;
        }


        return state;
    }

    private void Jump()
    {
        // Debug.Log("Jumping");
        // isWaiting = true;
        //// agent.enabled = false;
        // rigidbody.isKinematic = false;
        // rigidbody.useGravity = true;

        // rigidbody.AddRelativeForce(new Vector3(0, jumpHeight, 0), ForceMode.Impulse);

        Vector3 currentVelocity = agent.velocity;

        Debug.Log("Jumping");
        isWaiting = true;
        agent.enabled = false;
        rigidbody.isKinematic = false;
        rigidbody.useGravity = true;
        agent.transform.GetComponent<PatrolComponent>().isJumping = true;

        rigidbody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);

        rigidbody.velocity = currentVelocity;

        // You may want to re-enable gravity after a certain duration or height is reached
        // StartCoroutine(EnableGravityAfterDelay());
    }
}
public class PatrolTask : Node
{
    List<Transform> targets = new();
    NavMeshAgent agent;
    int targetIndx;
    float waitTime;
    float elapsedTime = 0;
    bool isWaiting = false;

    public PatrolTask(List<Transform> targets, NavMeshAgent agent, float waitTime)
    {
        this.targets = targets;
        this.agent = agent;
        this.waitTime = waitTime;
    }

    public override NodeState Evaluate()
    {
        state = NodeState.Running;
        

        if (isWaiting && targets.Count > 0)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= waitTime)
            {
                isWaiting = false;
                elapsedTime = 0;

                targetIndx = (targetIndx + 1) % targets.Count;
            }
        }
        else if (agent.enabled)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
                isWaiting = true;
            agent.enabled = true;

            if (!agent.SetDestination(agent.destination))
                state = NodeState.Failure;
        }

        return state;
    }
}