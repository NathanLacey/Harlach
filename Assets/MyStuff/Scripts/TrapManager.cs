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

    void Start()
    {
        for(int i = 0; i < transform.childCount; ++i)
        {
            Trap child = transform.GetChild(i).GetComponent<Trap>();
            if (child)
            {
                AllTraps.Add(child);
            }
        }
        TrapStartTimeController.Initialize(0.5f);
    }
    
    void Update()
    {
        if(CurrentTrapIndex < AllTraps.Count)
        {
            TrapStartTimeController.TimerAction(StartTrap);
        }
    }

    void StartTrap()
    {
        AllTraps[CurrentTrapIndex].Initialize();
        ++CurrentTrapIndex;
    }

}
