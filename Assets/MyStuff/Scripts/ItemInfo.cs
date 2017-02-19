using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class ItemInfo : MonoBehaviour
{
    [SerializeField]
    public MeshCollider mMeshCollider;

    Animator mAnimator;

    public float mAttackValue;
    public float mDefenseValue;
    // The higher it is the faster you will swing
    public float mAttackSpeed;
    public float mAnimationSpeed;

    public DamageType mModifierType;
    

	void Start ()
    {
        mMeshCollider = GetComponent<MeshCollider>();
        mAnimator = GetComponent<Animator>();
        RandomizeValues();
        mAnimator.SetFloat("AttackSpeed", mAnimationSpeed);
    }

    void RandomizeValues()
    {
        if(tag == "sword1h")
        {
            mAttackValue = Random.Range(1 + (SceneManager.GetActiveScene().buildIndex * 3), 10 + (SceneManager.GetActiveScene().buildIndex * 3));
            mDefenseValue = Random.Range(1 + (SceneManager.GetActiveScene().buildIndex * 3), 10 + (SceneManager.GetActiveScene().buildIndex * 3));
            mAttackSpeed = 0.0f; /*Random.Range(1.0f + 0.1f, 1.0f + 1.4f);*/
            mAnimationSpeed = 1.2f;
        }
        else if(tag == "sword2h")
        {
            mAttackValue = Random.Range(3 + (SceneManager.GetActiveScene().buildIndex * 3), 10 + (SceneManager.GetActiveScene().buildIndex * 3));
            mDefenseValue = Random.Range(3 + (SceneManager.GetActiveScene().buildIndex * 3), 10 + (SceneManager.GetActiveScene().buildIndex * 3));
            mAttackSpeed = 0.0f; /*Random.Range(1.0f + 0.1f, 1.0f + 1.4f);*/
            mAnimationSpeed = 0.7f;
        }
    }
	
	void Update ()
    {
	
	}
}
