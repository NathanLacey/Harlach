using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrapManager : MonoBehaviour
{
    [SerializeField]
    Timer TrapStartTimeController = new Timer();
    [SerializeField]
    List<Trap> AllTraps = new List<Trap>();
    int CurrentTrapIndex = 0;
    [SerializeField]
    float TimeBetweenTraps;

    void Awake()
    {
        for(int i = 0; i < transform.childCount; ++i)
        {
            Trap child = transform.GetChild(i).GetComponent<Trap>();
            if (child)
            {
                AllTraps.Add(child);
            }
        }
        TrapStartTimeController.Initialize(TimeBetweenTraps);
        enabled = false;
    }
    
    void Update()
    {

        if(CurrentTrapIndex < AllTraps.Count)
        {
            TrapStartTimeController.TimerAction(StartTrap);
        }
        else
        {
            enabled = false;
        }
    }

    void StartTrap()
    {
        AllTraps[CurrentTrapIndex].Initialize();
        ++CurrentTrapIndex;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player" && CurrentTrapIndex == 0)
        {
            enabled = true;
        }
    }
}
