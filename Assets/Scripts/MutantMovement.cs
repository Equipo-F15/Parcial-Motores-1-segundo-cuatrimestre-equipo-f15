using UnityEngine;
using UnityEngine.AI;

public class MutantMovement : LifeScript
{
   [SerializeField] private GameObject player;
   private NavMeshAgent navAgent; //agente nav mesh
   private Animator animator;

   public float patrolrange; //rango por el cual patrulla
   public Transform patrolCenterPoint;
   public bool playerChase;
    private MutantAttack attackScript;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>(); //agente nav mesh
        attackScript = GetComponent<MutantAttack>();
        animator = GetComponentInChildren<Animator>();
        playerChase = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerChase == false)
        {
            if (navAgent.velocity.magnitude != 0f) //recproduce animacion caminar si se mueve
            {
                animator.SetBool("walking", true);
            }
            else { animator.SetBool("walking", false); }

            //código patrullar inicio

            //para patrullar crea un punto aleatorio
            if (navAgent.remainingDistance <= navAgent.stoppingDistance)
            {
                Vector3 point;
                
                if (RandomPoint(transform.position, patrolrange, out point))
                {
                    Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //debug para ver
                    navAgent.SetDestination(point);
                }
            }
            //código patrullar fin
        }

        if (playerChase == true && attackScript.distanceToPlayer >= attackScript.distanceToAttack)
        {
            animator.SetBool("Running", true);
            //chase player script
            Vector3 playerPosition = player.transform.position;

            {
                Debug.DrawRay(playerPosition, Vector3.up, Color.blue, 1.0f); //debug para ver
                navAgent.SetDestination(playerPosition);

            }
        }

        if (currentLife <= 0)
        {
            animator.SetBool("isDead", true);
        }

    }


    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range; 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            result = hit.position;
            return true;
        }
        result = Vector3.zero;
        return false;        
    }

    void OnAnimatorMove()
    {
        if (animator.GetBool("walking"))
        {
            navAgent.speed = (animator.deltaPosition / Time.deltaTime).magnitude;
        }
    }
}
