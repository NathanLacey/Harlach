using UnityEngine;
using System.Collections;

public class AttackController : MonoBehaviour
{
    //
    // This is the player attackController for melee, bad naming conventions on my part
    //
    [SerializeField]
    Animator mAnimator;
    [SerializeField]
    ItemInfo mItem;
    AudioSource mAudioSource;

    void Awake()
    {
        mAnimator = gameObject.GetComponent<Animator>();
        mItem = gameObject.GetComponent<ItemInfo>();
        mAudioSource = gameObject.AddComponent<AudioSource>();
        mAudioSource.playOnAwake = false;
        if (mItem.mWeaponType == WeaponType.Sword)
        {
            mAudioSource.clip = AudioClips.Instance.mSwordSwing;
            LoadAnimator("Animators/SwordAnimator");
        }
        else if (mItem.mWeaponType == WeaponType.Wand)
        {
            mAudioSource.clip = AudioClips.Instance.mMagicCast;
            LoadAnimator("Animators/WandAnimator");
        }
        else if (mItem.mWeaponType == WeaponType.Shield)
        {
            mAudioSource.clip = AudioClips.Instance.mShieldBash;
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

    IEnumerator AttackSound(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        mAudioSource.Play();
    }

    public void Attack()
    {
        mAnimator.SetBool("IsAttacking", true);
        StartCoroutine(AttackAnimation());
        if(mItem.mWeaponType == WeaponType.Shield)
        {
            StartCoroutine(AttackSound(mItem.mAnimationWait * 0.5f));
        }
        else if (mItem.tag == "sword2h")
        {
            StartCoroutine(AttackSound(mItem.mAnimationWait * 0.4f));
        }
        else if(mItem.tag == "sword1h")
        {
            StartCoroutine(AttackSound(mItem.mAnimationWait * 0.25f));
        }
        else if (mItem.mWeaponType == WeaponType.Wand)
        {
            StartCoroutine(AttackSound(mItem.mAnimationWait * 0.2f));
            mItem.mItemMagicEffects.Attack();
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        //Debug.Log("Player Sword is Touching"+collider.name);
        if (mItem.mWeaponType != WeaponType.Wand)
        { 
            if (collider.isTrigger == false && collider.gameObject.GetComponent<Enemy>() != null && mAnimator.GetBool("IsAttacking"))
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
