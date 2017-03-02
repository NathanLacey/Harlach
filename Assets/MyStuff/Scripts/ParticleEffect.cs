using UnityEngine;
using System.Collections;

public class ParticleEffect : MonoBehaviour
{
    [SerializeField]
    ParticleSystem Effect;
    [SerializeField]
    ParticleSystem CloneParticleSystem;
    Timer ParticleEvaporationTimer = new Timer();
    [SerializeField]
    float LifetimeOfParticle;
    bool EvaporateParticleSystem = false;
    // Use this for initialization

    public void Initialize(Vector3 position, Quaternion rotation)
    {
        CloneParticleSystem = Instantiate(Effect, position, rotation) as ParticleSystem;
        Debug.Log(CloneParticleSystem);
        CloneParticleSystem.transform.localScale *= 3.0f;
        Play();
        EvaporateParticleSystem = false;
        ParticleEvaporationTimer.Initialize(LifetimeOfParticle);
    }

    public void Terminate(float waitTime = 0.0f)
    {
        if (CloneParticleSystem != null)
        {
            CloneParticleSystem.GetComponent<ParticleSystem>().Stop();
            Destroy(CloneParticleSystem, waitTime);
        }
    }

    public void Play()
    {
        CloneParticleSystem.GetComponent<ParticleSystem>().Play();
    }

    public void Stop()
    {
        CloneParticleSystem.GetComponent<ParticleSystem>().Stop();
    }

    public void ParticleTimerUpdate()
    {
        if (EvaporateParticleSystem == false)
        {
            ParticleEvaporationTimer.TimerAction(EvaporateParticle);
            if (EvaporateParticleSystem == true)
            {
                Stop();
            }
        }
    }

    void EvaporateParticle()
    {
        EvaporateParticleSystem = true;
    }
}
