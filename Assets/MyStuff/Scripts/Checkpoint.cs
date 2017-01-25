using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
    bool Active = false;

    public void SetCheckpoint()
    {
        Active = true;
    }

    public void UnSetCheckpoint()
    {
        Active = false;
    }

    public bool GetActive()
    {
        return Active;
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player")
        {
            CheckpointManager.Instance.ClearCheckpoints();
            SetCheckpoint();
        }
    }
}
