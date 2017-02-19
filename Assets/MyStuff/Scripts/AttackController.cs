using UnityEngine;
using System.Collections;

public class AttackController : MonoBehaviour
{
    //
    // This is the player attackController for melee, bad naming conventions on my part
    //
    [SerializeField]
    MeshCollider mMeshTrigger;
    [SerializeField]
    Animator mAnimator;
    [SerializeField]
    ItemInfo mItem;

    void Awake()
    {
        mMeshTrigger = gameObject.GetComponent<MeshCollider>();
        mAnimator = gameObject.GetComponent<Animator>();
        mItem = gameObject.GetComponent<ItemInfo>();
        mAnimator.runtimeAnimatorController = Resources.Load("Animators/SwordAnimator") as RuntimeAnimatorController;
        mAnimator.SetBool("FloatingItem", true);
    }

    void FixedUpdate()
    {
        if (mAnimator.GetBool("IsAttacking"))
        {
            StartCoroutine(AttackAnimation());

        }

    }
    IEnumerator AttackAnimation()
    {
        yield return new WaitForSeconds(mItem.mAttackSpeed);
        mAnimator.SetBool("IsAttacking", false);

    }

    public void Attack()
    {
        //Debug.Log(" Player Attack function is called");
        mAnimator.SetBool("IsAttacking", true);
    }
    
    void OnTriggerEnter(Collider collider)
    {

        //Debug.Log("Player Sword is Touching"+collider.name);
        if (collider.gameObject.GetComponent<Enemy>() != null && mAnimator.GetCurrentAnimatorStateInfo(0).IsName("OneHandedSword_Swing"))
        {
            Debug.Log("Player Sword is Damaging Enemy");
            collider.gameObject.GetComponent<Enemy>().Damage(mItem.mAttackValue, DamageType.Melee_Instance);
            collider.gameObject.GetComponent<Enemy>().SeesPlayer(transform);
        }

    }
}
