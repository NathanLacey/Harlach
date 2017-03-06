using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public const float mEnemyHealthMin = 10.0f;
    public const float mEnemyHealthMax = 20.0f;
    public const float mEnemyDefenceMin = 0.0f;
    public const float mEnemyDefenceMax = 5.0f;
    public const float mEnemyAttackMin = 3.0f;
    public const float mEnemyAttackMax = 5.0f;

    Timer AttackTimer = new Timer();
    Transform Target;
    bool IsBeingHit = false;
    private bool IsBleeding = false;
    private bool IsInvincible = false;
    private bool HasHeard = false;

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
    [SerializeField]
    WeaponAttack Sword;
    [SerializeField]
    Rigidbody MyRigidBody;
    [SerializeField]
    WanderBehaviour WanderScript;

    [Header("AI Values")]
    [SerializeField]
    float ViewDistance;
    [SerializeField]
    float SoundDistance;
    [SerializeField]
    float TurnSpeed;
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
    public float AttackValue;
    [SerializeField]
    float DefenceValue;

    void Start()
    {
        WanderScript = GetComponent<WanderBehaviour>();
        if (WanderScript)
            WanderScript.StopWandering();
        AttackTimer.Initialize(TimeBetweenAttacks);
        MyNavMeshAgent = GetComponent<NavMeshAgent>();
        MyNavMeshAgent.stoppingDistance = AttackingDistance - 1.5f;
        Trigger.size = new Vector3(ViewDistance, ViewDistance * 0.9f, ViewDistance);
        HealthProperty = MaxHealth;
        MyRigidBody = GetComponent<Rigidbody>();
        MyNavMeshAgent.updateRotation = true;
    }

    void OnTriggerStay(Collider collider)
    {
        if (Target == null && collider.gameObject.tag == "Player")
        {
            // negative is behind, positive is in front
            Vector3 dir = (collider.transform.position - transform.position).normalized;
            float direction = Vector3.Dot(dir, transform.forward);
            // For telling when the player is really close to be able to determine sound
            float distance = Vector3.Distance(collider.transform.position, transform.position);

            if(Mathf.Abs(distance) < SoundDistance)
            {
                HasHeard = true;
            }
            if (direction > 0)
            {
                SeesPlayer(collider.transform);
            }
            else if (HasHeard == true)
            {
                TurnTowardsPoint(collider.transform);
            }
            else
            {
                MyAnimator.SetBool("OnPath", false);
            }
            
        }
        else if(Target == null && WanderScript != null && WanderScript.IsWandering == false)
        {
            MyAnimator.SetBool("OnPath", true);
            WanderScript.enabled = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            if (Target == null && MyNavMeshAgent.destination != null)
            {
                LosePlayer();
            }
        }
    }

    void TurnTowardsPoint(Transform point)
    {
        StartCoroutine(BoolSwitchHasHeard(2.0f));
        MyAnimator.SetBool("OnPath", true);
        //MyNavMeshAgent.SetDestination(transform.position + transform.forward * -1.0f);
       // MyNavMeshAgent.SetDestination(transform.position + (point.position - transform.position));
        //var targetDir = point.position - transform.position;
        //var forward = transform.forward;
        //var localTarget = transform.InverseTransformPoint(point.position);

        //float angle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;

        //Vector3 eulerAngleVelocity = new Vector3(0, angle, 0);
        //Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.deltaTime * 5.0f);

        //MyRigidBody.MoveRotation(MyRigidBody.rotation * deltaRotation);

        //float rotationSpeed = 5.0f;
        //Vector3 direction = (point.position - transform.position).normalized;
        //Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));    // flattens the vector3
        //MyRigidBody.MoveRotation(Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed));
        transform.Rotate(Vector3.up, Time.deltaTime * TurnSpeed, Space.World);
    }

    public void SeesPlayer(Transform player)
    {
        Target = player;
        if(WanderScript)
            WanderScript.StopWandering();
        MyAnimator.SetBool("OnPath", true);
        MyAnimator.SetBool("SeesPlayer", true);
    }

    void LosePlayer()
    {
        Target = null;
        MyNavMeshAgent.Stop();
        MyAnimator.SetBool("OnPath", false);
        MyAnimator.SetBool("SeesPlayer", false);
    }

    void AttackPlayer()
    {
        MyAnimator.SetTrigger("AttackPlayer");
    }

    public void Damage(float amount, DamageType damageType, float invincibilityTime = 1.0f)
    {
        if (IsInvincible)
            return;
        StartCoroutine(InvincibilityWindow(invincibilityTime));
        MyAnimator.SetTrigger("GetHit");
        Debug.Log("Enemy Took damage: " + amount);
        switch (damageType)
        {
            case DamageType.Melee_Instance:
                amount -= DefenceValue;
                if (amount < 0.0f)
                    amount = 1.0f;
                HealthProperty -= amount;
                break;
            case DamageType.Melee_Bleeding:
                amount -= DefenceValue;
                if (amount < 0.0f)
                    amount = 1.0f;
                HealthProperty -= amount;
                IsBleeding = true;
                break;
            case DamageType.Magic_Instance:
                HealthProperty -= amount;
                break;
            default:
                Debug.Log("[Enemy::Damage] Invalid damage type");
                break;
        }
        StartCoroutine(BoolSwitchIsBeingHit(0.5f));
    }

    void Die()
    {
        DeathEffect.Initialize(transform.position, transform.rotation);
        if (ItemDropped)
        {
            ItemDropped = (ItemPickup)Instantiate(ItemDropped, transform.position, transform.rotation);
            ItemSpawner.Instance.SpawnRandomItem(transform, ItemDropped, ItemType);
        }
        MyAnimator.SetBool("Dead", true);
        StartCoroutine(DeathWait(5.0f));
    }

    void Shrink()
    {
        transform.localScale *= 0.975f;
    }

    IEnumerator InvincibilityWindow(float waitTime)
    {
        IsInvincible = true;
        yield return new WaitForSeconds(waitTime);
        IsInvincible = false;
    }

    IEnumerator DeathWait(float waitTime)
    {
        InvokeRepeating("Shrink", 0.0f, 0.075f);
        yield return new WaitForSeconds(waitTime);
        if(DeathEffect)
            DeathEffect.Terminate(waitTime);
        Destroy(gameObject);
    }

    IEnumerator BoolSwitchIsBeingHit(float waitTime)
    {
        IsBeingHit = true;
        yield return new WaitForSeconds(waitTime);
        IsBeingHit = false;
    }

    IEnumerator BoolSwitchHasHeard(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        HasHeard = false;
    }

    void Update()
    {
        if(Target && Health > 0.0f)
        {
            MyNavMeshAgent.SetDestination(Target.position);

            float distance = Vector3.Distance(Target.transform.position, transform.position);

            if (Mathf.Abs(distance) > ViewDistance)
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

    public void GenerateValues(Room.RoomDifficulty difficulty)
    {
        MaxHealth = Random.Range(mEnemyHealthMin, mEnemyHealthMax) * (float)difficulty;
        HealthProperty = MaxHealth;
        DefenceValue = Random.Range(mEnemyDefenceMin, mEnemyDefenceMax) * (float)difficulty;
        AttackValue = Random.Range(mEnemyAttackMin, mEnemyAttackMax) * (float)difficulty;

        Sword.SetDamage();
    }

}
