using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public enum WeaponType { Sword, Wand, Shield };
public class ItemInfo : MonoBehaviour
{

    [SerializeField]
    public MeshCollider mMeshCollider;

    Animator mAnimator;

    public float mAttackValue;
    public float mDefenseValue;
    // The higher it is the faster you will swing
    public float mAttackSpeed;
    public float mAnimationWait;
    public MagicEffects mItemMagicEffects;
    public DamageType mModifierType;
    public WeaponType mWeaponType;

	void Start ()
    {
        mMeshCollider = GetComponent<MeshCollider>();
        mAnimator = GetComponent<Animator>();
        RandomizeValues();
        mAnimator.SetFloat("AttackSpeed", mAttackSpeed);
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
        }
    }
    void RandomizeValues()
    {
        if (tag == "sword1h")
        {
            mAttackValue = Random.Range(1 + (SceneManager.GetActiveScene().buildIndex * 3), 10 + (SceneManager.GetActiveScene().buildIndex * 3));
            mDefenseValue = 0.0f;
            mAttackSpeed = 1.2f; /*Random.Range(1.0f + 0.1f, 1.0f + 1.4f);*/
            mAnimationWait = 1.0f;
        }
        else if (tag == "sword2h")
        {
            mAttackValue = Random.Range(3 + (SceneManager.GetActiveScene().buildIndex * 3), 10 + (SceneManager.GetActiveScene().buildIndex * 3));
            mDefenseValue = Random.Range(1 + (SceneManager.GetActiveScene().buildIndex * 3), 5 + (SceneManager.GetActiveScene().buildIndex * 3));
            mAttackSpeed = 0.7f;
            mAnimationWait = 1.0f;
        }
        else if(tag == "wand")
        {
            mAttackValue = Random.Range(1 + (SceneManager.GetActiveScene().buildIndex * 3), 10 + (SceneManager.GetActiveScene().buildIndex * 3));
            mDefenseValue = 0.0f;
            mAttackSpeed = 1.0f;
            mAnimationWait = 0.5f;
        }
        else if(tag == "shield")
        {
            mAttackValue = Random.Range(1 + (SceneManager.GetActiveScene().buildIndex * 3), 5 + (SceneManager.GetActiveScene().buildIndex * 3));
            mDefenseValue = Random.Range(3 + (SceneManager.GetActiveScene().buildIndex * 3), 10 + (SceneManager.GetActiveScene().buildIndex * 3));
            mAttackSpeed = 0.9f;
            mAnimationWait = 0.5f;
        }
        
    }

    public bool IsSword()
    {
        return tag == "sword1h" || tag == "sword2h";
    }
}
