using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour
{
    [SerializeField]
    Timer MyTimer = new Timer();
    [SerializeField]
    float TimeLeft;
    [SerializeField]
    Animator MyAnimator;
    bool StartTrap = false;

    public void Initialize()
    {
        StartTrap = true;
        MyTimer.Initialize(3.0f);
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
