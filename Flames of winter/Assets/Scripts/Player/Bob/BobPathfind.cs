using UnityEngine;
using UnityEngine.AI;

public class BobPathfind : MonoBehaviour
{
    [SerializeField] private float angularSpeed = 90f;
    [SerializeField] private float angleEpsilon = 1f;
    [SerializeField] GameObject pingPrefab;

    private CharacterController controller;
    private NavMeshAgent agent;

    private bool path = false;
    private Transform target;
    private Vector3 location;
    private GameObject ping;
    private bool startedMoving = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
    }

    void FixedUpdate()
    {
        if (path)
        {
            agent.destination = target ? target.position : location;
            if (agent.path.corners.Length > 1)
            {
                if (!target && (!ping || !ping.transform.position.Equals(agent.path.corners[^1])))
                {
                    if (ping)
                        Destroy(ping);
                    ping = Instantiate(pingPrefab, agent.path.corners[^1], Quaternion.Euler(0, 0, 0));
                }
                Vector3 desired = agent.path.corners[1] - transform.position;
                desired.y = 0;
                Vector3 forward = Vector3.RotateTowards(transform.forward, desired, angularSpeed * Mathf.Deg2Rad * Time.fixedDeltaTime, 0f);
                transform.rotation = Quaternion.LookRotation(forward);

                float angle = Vector3.Angle(transform.forward, desired);
                if (angle < angleEpsilon)
                {
                    transform.rotation = Quaternion.LookRotation(desired);
                    agent.isStopped = false;
                    startedMoving = true;
                }
                else if (startedMoving)
                {
                    agent.velocity = agent.speed * Mathf.Pow(Mathf.Max(Mathf.Cos(angle * Mathf.Deg2Rad), 0), 4) * transform.forward;
                    agent.isStopped = true;
                }
                else
                {
                    agent.velocity = Vector3.zero;
                    agent.isStopped = true;
                }
            }

            if (ping && !agent.hasPath && agent.remainingDistance <= agent.stoppingDistance)
                StopPathing();
        }
    }

    /*public void FollowTarget(Transform target)
    {
        StopPathing();
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
    }*/

    public void PathTo(Vector3 location)
    {
        StopPathing();
        if (!NavMesh.SamplePosition(transform.position - new Vector3(0, 0.5f, 0), out _, 0.1f, NavMesh.AllAreas))
            return;
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
        if (ping)
            Destroy(ping);
        ping = null;
        path = false;
        agent.enabled = false;
        controller.enabled = true;
        startedMoving = false;
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