using UnityEngine;
using System.Collections;

public class AudioClips : MonoBehaviour
{
    private AudioClips() { }
    static private AudioClips TheInstance;
    static public AudioClips Instance
    {
        get
        {
            return TheInstance;
        }
    }
    // Use this for initialization
    void Awake()
    {
        TheInstance = this;
    }

    public AudioClip mSwordSwing;
    public AudioClip mShieldBash;
    public AudioClip mMagicCast;
    public AudioClip mSkeletonDeath;
}
