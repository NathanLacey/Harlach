using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CheckpointManager : MonoBehaviour
{
    private static CheckpointManager TheInstance;
    public static CheckpointManager Instance
    {
        get
        {
            return TheInstance;
        }
    }
    private CheckpointManager() { }

    [SerializeField]
    List<Checkpoint> AllCheckpoints = new List<Checkpoint>();
   
    void Awake()
    {
        TheInstance = this;

        for (int i = 0; i < transform.childCount; ++i)
        {
            Checkpoint child = transform.GetChild(i).GetComponent<Checkpoint>();
            if (child)
            {
                AllCheckpoints.Add(child);
            }
        }
        if(AllCheckpoints.Count > 0)
        {
            AllCheckpoints[0].SetCheckpoint();
        }
    }
    
    public void ClearCheckpoints()
    {
        foreach(Checkpoint checkpoint in AllCheckpoints)
        {
            checkpoint.UnSetCheckpoint();
        }
    }

    public Checkpoint GetCurrentCheckpoint()
    {
        foreach (Checkpoint checkpoint in AllCheckpoints)
        {
            if(checkpoint.GetActive())
            {
                return checkpoint;
            }
        }
        Debug.Log("[CheckpointManager::GetCurrentCheckpoint] No checkpoint is set");
        return null;
    }
}
