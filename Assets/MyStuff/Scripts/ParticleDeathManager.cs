using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleDeathManager : MonoBehaviour
{
    public class ParticleDeath
    {
        public ParticleDeath(ParticleSystem p, float timer)
        {
            particle = p;
            deathTimer.Initialize(timer);
            lifeIsOver = false;
        }
        public ParticleSystem particle;
        public Timer deathTimer = new Timer();
        public bool lifeIsOver;
    }
    private ParticleDeathManager() { }
    private static ParticleDeathManager TheInstance;
    public static ParticleDeathManager Instance
    {
        get
        {
            return TheInstance;
        }
    }

    public List<ParticleDeath> mParticleList = new List<ParticleDeath>();

    void Awake()
    {
        TheInstance = this;
    }

    void Update()
    {
        if(mParticleList.Count > 0)
        {
            for(int i = 0; i < mParticleList.Count; ++i)
            {
                mParticleList[i].deathTimer.TimerAction(SetParticleLifeBool, mParticleList[i]);
                if(mParticleList[i].lifeIsOver == true)
                {
                    RemoveParticle(mParticleList[i]);
                    --i;
                }
            }
        }
    }

    public void AddParticle(ParticleSystem particle, float deathTime)
    {
        mParticleList.Add(new ParticleDeath(particle, deathTime));
    }

    void SetParticleLifeBool(ParticleDeath particle)
    {
        particle.lifeIsOver = true;
    }

    void RemoveParticle(ParticleDeath particle)
    {
        mParticleList.Remove(particle);
        Destroy(particle.particle.gameObject);
    }
    
}
