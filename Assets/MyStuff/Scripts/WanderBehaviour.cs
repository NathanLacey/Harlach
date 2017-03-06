using UnityEngine;
using System.Collections;

public class WanderBehaviour : MonoBehaviour
{
    public bool IsWandering;
    public float mWanderRadius;
    public float mWanderTimer;

    private Transform target;
    private NavMeshAgent agent;
    private float timer;

    public void StopWandering()
    {
        IsWandering = false;
        enabled = false;
    }

    // Use this for initialization
    void OnEnable()
    {
        IsWandering = true;
        agent = GetComponent<NavMeshAgent>();
        timer = mWanderTimer;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= mWanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, mWanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}
