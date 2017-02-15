using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    Timer AttackTimer = new Timer();
    Transform Target;
    bool IsBeingHit = false;

    [Header("Non-EnemyValues")]
    [SerializeField]
    ParticleEffect DeathEffect;
    [SerializeField]
    ItemPickup ItemDropped;
    [SerializeField]
    string ItemType;

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
    float HealthProperty
    {
        get
        {
            return Health;
        }
        set
        {
            Health = value;
            if(Health <= 0)
            {
                Health = 0;
                Die();
            }
        }
    }
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
        HealthProperty = MaxHealth;
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
        Damage(100);
    }

    public void Damage(float amount)
    {
        MyAnimator.SetTrigger("GetHit");
        
        HealthProperty -= amount;
        StartCoroutine(BoolSwitchIsBeingHit(0.5f));
    }

    void Die()
    {
        DeathEffect.Initialize(transform.position, transform.rotation);
        ItemDropped = (ItemPickup)Instantiate(ItemDropped, transform.position, transform.rotation);
        ItemSpawner.Instance.SpawnRandomItem(transform, ItemDropped, ItemType);

        MyAnimator.SetBool("Dead", true);
        StartCoroutine(DeathWait(5.0f));
    }

    void Shrink()
    {
        transform.localScale *= 0.975f;
    }

    IEnumerator DeathWait(float waitTime)
    {
        InvokeRepeating("Shrink", 0.0f, 0.075f);
        yield return new WaitForSeconds(waitTime);
        DeathEffect.Terminate(waitTime);
        Destroy(gameObject);
    }

    IEnumerator BoolSwitchIsBeingHit(float waitTime)
    {
        IsBeingHit = true;
        yield return new WaitForSeconds(waitTime);
        IsBeingHit = false;
    }

    void Update()
    {
        if(Target && Health > 0.0f)
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
