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
    }

    public void LoadAnimator(string path)
    {
        mAnimator.runtimeAnimatorController = Resources.Load(path) as RuntimeAnimatorController;
    }

    IEnumerator AttackAnimation()
    {
        yield return new WaitForSeconds(mItem.mAnimationWait);
        mAnimator.SetBool("IsAttacking", false);
    }

    public void Attack()
    {
        mAnimator.SetBool("IsAttacking", true);
        StartCoroutine(AttackAnimation());

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
                collider.gameObject.GetComponent<Enemy>().Damage(mItem.mAttackValue, mItem.mModifierType);
                collider.gameObject.GetComponent<Enemy>().SeesPlayer(transform);
            }
        }
    }

    public bool CanAttack()
    {
        return mAnimator.GetBool("IsAttacking") == false;
    }

    public bool IsSword()
    {
        return mItem.IsSword();
    }

    public float GetDefenceValue()
    {
        return mItem.mDefenseValue;
    }

    public float GetAttackValue()
    {
        return mItem.mAttackValue;
    }
}
