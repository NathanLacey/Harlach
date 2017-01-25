using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Trap : MonoBehaviour
{
    Timer MyTimer = new Timer();
    [SerializeField]
    float TimeInterval;
    [SerializeField]
    List<Animator> MyAnimator = new List<Animator>();
    bool StartTrap = false;
    
    public void Initialize()
    {
        StartTrap = true;
        MyTimer.Initialize(TimeInterval);
        ChangeAnimationState();
    }

    void Update()
    {
        if (!StartTrap)
            return;

        MyTimer.TimerAction(ChangeAnimationState);
    }

    void ChangeAnimationState()
    {
        foreach (Animator animatedObject in MyAnimator)
        {
            animatedObject.SetTrigger("Change");
        }
    }

    //void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("here");
    //    if(collision.gameObject.tag == "Player")
    //    {
    //        Debug.Log("here2");
    //        Player playerHit = collision.transform.GetComponent<Player>();

    //        playerHit.Damage(5.0f, DamageType.Melee_Bleeding, 0.5f);
    //    }
    //}
}
