using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    ParticleEffect ParticleSystemToSpawn;
    bool Active = false;


    void Update()
    {
        if(Active == true)
        {
            ParticleSystemToSpawn.ParticleTimerUpdate();
        }
    }

    public void SetCheckpoint()
    {
        ParticleSystemToSpawn.Initialize(transform.position, transform.rotation);
        Active = true;
    }

    public void UnSetCheckpoint()
    {
        ParticleSystemToSpawn.Terminate();
        Active = false;
    }

    public bool GetActive()
    {
        return Active;
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player" && Active == false)
        {
            CheckpointManager.Instance.ClearCheckpoints();
            SetCheckpoint();
        }
    }
}
