using UnityEngine;
using UnityEngine.AI;

public class BobPathfind : MonoBehaviour
{
    [SerializeField] private float angularSpeed = 90f;
    [SerializeField] private float angleEpsilon = 1f;

    private CharacterController controller;
    private NavMeshAgent agent;

    private bool path = false;
    private Transform target;
    private Vector3 location;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
    }

    void Update()
    {
        if (path)
        {
            agent.destination = target ? target.position : location;
            if (agent.path.corners.Length > 1)
            {
                Vector3 desired = agent.path.corners[1] - transform.position;
                desired.y = 0;
                Vector3 forward = Vector3.RotateTowards(transform.forward, desired, angularSpeed * Mathf.Deg2Rad * Time.deltaTime, 0f);
                transform.rotation = Quaternion.LookRotation(forward);

                if (Vector3.Angle(transform.forward, desired) < angleEpsilon)
                {
                    transform.rotation = Quaternion.LookRotation(desired);
                    agent.isStopped = false;
                }
                else
                {
                    agent.velocity = Vector3.zero;
                    agent.isStopped = true;
                }
            }

            if (!target && !agent.pathPending && !agent.hasPath && agent.remainingDistance <= agent.stoppingDistance)
                StopPathing();
        }
    }

    public void FollowTarget(Transform target)
    {
        if (agent.enabled)
        {
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
        }
        this.target = target;
        agent.stoppingDistance = 1.5f;
        controller.enabled = false;
        agent.enabled = true;
        path = true;
    }

    public void PathTo(Vector3 location)
    {
        if (agent.enabled)
        {
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
        }
        this.target = null;
        this.location = location;
        agent.stoppingDistance = 0f;
        controller.enabled = false;
        agent.enabled = true;
        path = true;
    }

    public void StopPathing()
    {
        path = false;
        agent.enabled = false;
        controller.enabled = true;
    }

    public bool IsPathing()
    {
        return path;
    }

    public bool IsFollowing()
    {
        return path && target;
    }
}