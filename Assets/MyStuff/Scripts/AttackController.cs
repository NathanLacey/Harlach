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


    public float mMaxCoolDown;
    public float mAttackCoolDown;


    void Awake()
    {
        mMeshTrigger = gameObject.GetComponent<MeshCollider>();
        mAnimator = gameObject.GetComponent<Animator>();
        mItem = gameObject.GetComponent<ItemInfo>();
        if (mItem.mWeaponType == WeaponType.Sword)
        {
            LoadAnimator("Animators/SwordAnimator");
        }
        else if (mItem.mWeaponType == WeaponType.Wand)
        {
            LoadAnimator("Animators/WandAnimator");
        }
        else if (mItem.mWeaponType == WeaponType.Shield)
        {
            LoadAnimator("Animators/ShieldAnimator");
        }

        mMaxCoolDown = 1.5f;
    }

    public void LoadAnimator(string path)
    {
        mAnimator.runtimeAnimatorController = Resources.Load(path) as RuntimeAnimatorController;
    }

    void FixedUpdate()
    {
        if (mAnimator.GetBool("IsAttacking"))
        {
            StartCoroutine(AttackAnimation());
        }

        if (mAttackCoolDown <= 0 && Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (mItem.mWeaponType == WeaponType.Wand)
            {
               Attack();
            }
            mAttackCoolDown = mMaxCoolDown;
        }
        mAttackCoolDown -= Time.deltaTime;

    }
    IEnumerator AttackAnimation()
    {
        yield return new WaitForSeconds(mItem.mAttackSpeed);
        mAnimator.SetBool("IsAttacking", false);

    }

    public void Attack()
    {
        mAnimator.SetBool("IsAttacking", true);

        if (mItem.mWeaponType == WeaponType.Wand)
        {
            mItem.mItemMagicEffects.Attack();
        }


    }

    void OnTriggerEnter(Collider collider)
    {
        //Debug.Log("Player Sword is Touching"+collider.name);
        if (mItem.mWeaponType != WeaponType.Wand)
        {
            if (collider.gameObject.GetComponent<Enemy>() != null && mAnimator.GetBool("IsAttacking"))
            {
                //Debug.Log("Player is Damaging Enemy");
                collider.gameObject.GetComponent<Enemy>().Damage(mItem.mAttackValue, DamageType.Melee_Instance);
                collider.gameObject.GetComponent<Enemy>().SeesPlayer(transform);
            }
        }

    }

    public bool IsSword()
    {
        return mItem.IsSword();
    }
}
