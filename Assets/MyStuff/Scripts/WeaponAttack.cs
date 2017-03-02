using UnityEngine;
using System.Collections;

public class WeaponAttack : MonoBehaviour
{
    //
    // This is the enemy attackController for melee, bad naming conventions on my part
    //
    Transform mParent;
    MeshCollider WeaponTrigger;

    [SerializeField]
    Animator mAnimator;
    [SerializeField]
    float mDamage;

    void Awake()
    {
        WeaponTrigger = GetComponent<MeshCollider>();
        Transform current = transform;
        Transform parent = transform.parent;
        while(parent != null)
        {
            current = parent;
            parent = parent.parent;
        }
        mParent = current;

        SetDamage();
    }

    public void SetDamage()
    {
        mDamage = mParent.GetComponent<Enemy>().AttackValue;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player" && mAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            Debug.Log("Enemy Sword is hitting player");
            collider.gameObject.GetComponent<Player>().Damage(mDamage, DamageType.Melee_Instance);
        }
    }


}


