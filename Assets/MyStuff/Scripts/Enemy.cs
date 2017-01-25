using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    Transform Target;
    [SerializeField]
    Animator MyAnimator;
    NavMeshAgent MyNavMeshAgent;

    void Start()
    {
        MyNavMeshAgent = GetComponent<NavMeshAgent>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            Target = collider.transform;
            MyAnimator.SetBool("OnPath", true);
            MyAnimator.SetBool("SeesPlayer", true);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            Target = null;
            MyAnimator.SetBool("OnPath", false);
            MyAnimator.SetBool("SeesPlayer", false);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("TEst");
            MyAnimator.SetTrigger("AttackPlayer");
        }
    }

    void Update()
    {
        if(Target)
        {
            MyNavMeshAgent.SetDestination(Target.position + Target.forward* 2.0f);
        }
    }
}
