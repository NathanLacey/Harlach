using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MagicEffects : MonoBehaviour
{
    public ParticleSystem mPrefabParticle;
    //ParticleSystem mParticleSystem;
    public List<ParticleCollisionEvent> mCollisionEvents;
    [SerializeField]
    ItemInfo mItem;
    //Transform mSavedTransform;
    List<ParticleSystem> mParticleList = new List<ParticleSystem>();
    void Awake()
    {
        mItem = transform.GetComponentInParent<ItemInfo>();

        //mPrefabParticle.startLifetime = 1.0f;
        //mParticleList.AddRange(Resources.LoadAll<ParticleSystem>("MagicEffectsFolder"));
        for(int i = 0; i < transform.childCount; i++)
        {
            mParticleList.Add(transform.GetChild(i).GetComponent<ParticleSystem>());
        }
        mPrefabParticle = mParticleList[Random.Range(0, mParticleList.Count)];

        //mPrefabParticle = (ParticleSystem)Instantiate(mPrefabParticle);
        mPrefabParticle.transform.rotation = Quaternion.Euler(-45, 0, 0);
        mCollisionEvents = new List<ParticleCollisionEvent>();
    }

    // Update is called once per frame
    void Update()
    {
        //mPrefabParticle.transform.rotation = mSavedTransform.rotation;
    }

    //plays the particle system effect on for the attack
    public void Attack()
    {
        mPrefabParticle.Emit(1);
    }

    //when the particle collides with something

}
