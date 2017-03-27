using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleCollision : MonoBehaviour
{
    public ParticleSystem mPrefabParticle;
    public List<ParticleCollisionEvent> mCollisionEvents;

    [SerializeField]
    ItemInfo mItem;
    // Use this for initialization
    void Awake()
    {
        mPrefabParticle = GetComponent<ParticleSystem>();
        mItem = transform.parent.parent.GetComponent<ItemInfo>();
    }

    void OnParticleCollision(GameObject target)
    {
        //Debug.Log("Checking collision Events");
        //if it doesn't have an Enemy Script then ignore it
        if (target.GetComponent<Enemy>() != null)
        {
            target.GetComponent<Enemy>().Damage(mItem.mAttackValue, mItem.mModifierType);
        }
    }
}
