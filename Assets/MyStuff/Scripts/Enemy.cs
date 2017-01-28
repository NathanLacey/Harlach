using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    Timer AttackTimer = new Timer();
    Transform Target;
    bool IsBeingHit = false;

    [Header("Component Values")]
    [SerializeField]
    Animator MyAnimator;
    NavMeshAgent MyNavMeshAgent;
    [SerializeField]
    BoxCollider Trigger;

    [Header("AI Values")]
    [SerializeField]
    float ViewDistance;
    [SerializeField]
    float AttackingDistance;
    [SerializeField]
    float TimeBetweenAttacks;

    [Header("Stat Values")]
    [SerializeField]
    float MaxHealth;
    [SerializeField]
    float Health;
    [SerializeField]
    float AttackValue;
    [SerializeField]
    float DefenceValue;

    void Start()
    {
        AttackTimer.Initialize(TimeBetweenAttacks);
        MyNavMeshAgent = GetComponent<NavMeshAgent>();
        MyNavMeshAgent.stoppingDistance = AttackingDistance - 1.5f;
        Trigger.size = new Vector3(ViewDistance, ViewDistance * 0.9f, ViewDistance);
        Health = MaxHealth;
    }

    void OnTriggerStay(Collider collider)
    {
        Vector3 dir = (collider.transform.position - transform.position).normalized;
        float direction = Vector3.Dot(dir, transform.forward);

        if(Target == null && collider.gameObject.tag == "Player" && direction > 0)
        {
            Target = collider.transform;
            MyAnimator.SetBool("OnPath", true);
            MyAnimator.SetBool("SeesPlayer", true);
        }
    }

    void LosePlayer()
    {
        Target = null;
        MyAnimator.SetBool("OnPath", false);
        MyAnimator.SetBool("SeesPlayer", false);
    }

    void AttackPlayer()
    {
        MyAnimator.SetTrigger("AttackPlayer");

        Player playerTarget = Target.GetComponent<Player>();
        playerTarget.Damage(AttackValue, DamageType.Melee_Instance);
    }

    public void Damage(float amount)
    {
        MyAnimator.SetTrigger("GetHit");

        Health -= amount;
        StartCoroutine(BoolSwitchIsBeingHit(0.5f));
    }

    IEnumerator BoolSwitchIsBeingHit(float waitTime)
    {
        IsBeingHit = true;
        yield return new WaitForSeconds(waitTime);
        IsBeingHit = false;
    }

    void Update()
    {
        if(Target)
        {
            MyNavMeshAgent.SetDestination(Target.position);

            float distance = Vector3.Distance(Target.transform.position, transform.position);

            if (distance > ViewDistance)
            {
                LosePlayer();
            }
            else if(distance < AttackingDistance && !IsBeingHit)
            {
                AttackTimer.TimerAction(AttackPlayer);
            }
            else if(MyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                MyAnimator.SetTrigger("ChasePlayer");
            }
        }
    }
}
