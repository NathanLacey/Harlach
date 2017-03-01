using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public enum WeaponType { Sword, Wand, Shield};
public class ItemInfo : MonoBehaviour
{

    [SerializeField]
    public MeshCollider mMeshCollider;

    Animator mAnimator;

    public float mAttackValue;
    public float mDefenseValue;
    // The higher it is the faster you will swing
    public float mAttackSpeed;
    public float mAnimationSpeed;
    public MagicEffects mItemMagicEffects;
    public DamageType mModifierType;
    public WeaponType mWeaponType;

	void Start ()
    {
        mMeshCollider = GetComponent<MeshCollider>();
        mAnimator = GetComponent<Animator>();
        mAttackValue = Random.Range(1 + (SceneManager.GetActiveScene().buildIndex * 3), 10 + (SceneManager.GetActiveScene().buildIndex * 3));
        mDefenseValue = Random.Range(1 + (SceneManager.GetActiveScene().buildIndex * 3), 10 + (SceneManager.GetActiveScene().buildIndex * 3));
        mAttackSpeed = 0.0f; /*Random.Range(1.0f + 0.1f, 1.0f + 1.4f);*/
        mAnimationSpeed = 0.7f;
        mAnimator.SetFloat("AttackSpeed", mAnimationSpeed);
        mModifierType = DamageType.Melee_Instance; /*(DamageType)Random.Range(0, ????????);change later to max size of enum */
        if(IsSword())
        {
            mWeaponType = WeaponType.Sword;
        }
        else if(gameObject.tag == "wand")
        {
            mWeaponType = WeaponType.Wand;
            mItemMagicEffects = transform.GetChild(0).GetComponent<MagicEffects>();

        }
        else if(gameObject.tag == "shield")
        {
            mWeaponType = WeaponType.Shield;
            mAttackValue *= 0.5f;
        }
    }
    void RandomizeValues()
    {
        if (tag == "sword1h")
        {
            mAttackValue = Random.Range(1 + (SceneManager.GetActiveScene().buildIndex * 3), 10 + (SceneManager.GetActiveScene().buildIndex * 3));
            mDefenseValue = Random.Range(1 + (SceneManager.GetActiveScene().buildIndex * 3), 10 + (SceneManager.GetActiveScene().buildIndex * 3));
            mAttackSpeed = 0.0f; /*Random.Range(1.0f + 0.1f, 1.0f + 1.4f);*/
            mAnimationSpeed = 1.2f;
        }
        else if (tag == "sword2h")
        {
            mAttackValue = Random.Range(3 + (SceneManager.GetActiveScene().buildIndex * 3), 10 + (SceneManager.GetActiveScene().buildIndex * 3));
            mDefenseValue = Random.Range(3 + (SceneManager.GetActiveScene().buildIndex * 3), 10 + (SceneManager.GetActiveScene().buildIndex * 3));
            mAttackSpeed = 0.0f; /*Random.Range(1.0f + 0.1f, 1.0f + 1.4f);*/
            mAnimationSpeed = 0.7f;
        }
    }

    public bool IsSword()
    {
        return tag == "sword1h" || tag == "sword2h";
    }
}
