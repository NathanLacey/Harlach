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
}
