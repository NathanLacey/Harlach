﻿using UnityEngine;
using System.Collections;

public class WeaponAttack : MonoBehaviour
{
    //
    // This is the enemy attackController for melee, bad naming conventions on my part
    //

    MeshCollider WeaponTrigger;

    [SerializeField]
    Animator mAnimator;
    [SerializeField]
    float damage;

    void Start()
    {
        WeaponTrigger = GetComponent<MeshCollider>();
    }

    void Update()
    {
        
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player" && mAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            Debug.Log("Enemy Sword is hitting player");
            collider.gameObject.GetComponent<Player>().Damage(damage, DamageType.Melee_Instance);
        }
    }


}


