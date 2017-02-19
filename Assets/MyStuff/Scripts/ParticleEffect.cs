using UnityEngine;
using System.Collections;

public class ParticleEffect : MonoBehaviour
{
    [SerializeField]
    ParticleSystem Effect;
    ParticleSystem CloneParticleSystem;
    Timer ParticleEvaporationTimer = new Timer();
    [SerializeField]
    float LifetimeOfParticle;
    bool EvaporateParticleSystem = false;
    // Use this for initialization

    public void Initialize(Vector3 position, Quaternion rotation)
    {
        CloneParticleSystem = (ParticleSystem)Instantiate(Effect, position, rotation);
        CloneParticleSystem.transform.localScale *= 3.0f;
        CloneParticleSystem.Play();
        EvaporateParticleSystem = false;
        ParticleEvaporationTimer.Initialize(LifetimeOfParticle);
    }

    public void Terminate(float waitTime = 0.0f)
    {
        if (CloneParticleSystem != null)
        {
            Debug.Log("Terminating");
            CloneParticleSystem.Stop();
            Destroy(CloneParticleSystem.gameObject, waitTime);
        }
    }

    public void Play()
    {
        CloneParticleSystem.Play();
    }

    public void Stop()
    {
        CloneParticleSystem.Stop();
    }

    public void ParticleTimerUpdate()
    {
        if (EvaporateParticleSystem == false)
        {
            ParticleEvaporationTimer.TimerAction(EvaporateParticle);
            if (EvaporateParticleSystem == true)
            {
                CloneParticleSystem.Stop();
            }
        }
    }

    void EvaporateParticle()
    {
        EvaporateParticleSystem = true;
    }
}
