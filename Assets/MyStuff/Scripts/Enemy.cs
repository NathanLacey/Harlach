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

    void OnTriggerStay(Collider collider)
    {
        Vector3 dir = (collider.transform.position - transform.position).normalized;
        float direction = Vector3.Dot(dir, transform.forward);

        if(Target == null && collider.gameObject.tag == "Player" && direction > 0)
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
           // float distance = Vector3.Distance(Target.transform.position, transform.position);
            
            MyNavMeshAgent.SetDestination(Target.position + Target.forward* 2.0f);
        }
    }
}
