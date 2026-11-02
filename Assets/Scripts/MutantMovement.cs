using UnityEngine;
using UnityEngine.AI;

public class MutantMovement : MonoBehaviour
{
    
   private NavMeshAgent navAgent;
   private Animator animator;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator= GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (navAgent.velocity.magnitude != 0f)
        {
            animator.SetBool("walking", true);
        }
        else { animator.SetBool("walking", false); }
               
    }

    void OnAnimatorMove()
    {
        if (animator.GetBool("walking"))
        {
            navAgent.speed = (animator.deltaPosition / Time.deltaTime).magnitude;
        }
    }
}
