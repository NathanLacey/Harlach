using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour
{
    Timer MyTimer = new Timer();
    [SerializeField]
    float TimeInterval;
    [SerializeField]
    Animator MyAnimator;
    bool StartTrap = false;
    
    public void Initialize()
    {
        StartTrap = true;
        MyTimer.Initialize(TimeInterval);
    }

    void Update()
    {
        if (!StartTrap)
            return;

        MyTimer.TimerAction(ChangeAnimationState);
    }

    void ChangeAnimationState()
    {
        MyAnimator.SetTrigger("Change");
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
