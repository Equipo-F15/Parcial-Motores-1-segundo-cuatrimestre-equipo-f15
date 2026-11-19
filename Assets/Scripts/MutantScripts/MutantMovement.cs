using UnityEngine;
using UnityEngine.AI;

public class MutantChaseScript : LifeScript
{
   [SerializeField] private GameObject player;
   private NavMeshAgent navAgent; //agente nav mesh
   private Animator animator;

 
    public bool playerChase;
    private MutantAttack attackScript;

    MutantPatrolScript patrolScript;
    bool isAllucination;


    [SerializeField] GameObject alert;
    [SerializeField] VariacionSFX alertAudioVarSource;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>(); //agente nav mesh
        attackScript = GetComponent<MutantAttack>();
        animator = GetComponentInChildren<Animator>();
        alertAudioVarSource = alert.GetComponent<VariacionSFX>();

        patrolScript = GetComponent<MutantPatrolScript>();
        if (patrolScript != null)
        {
            playerChase = false;
        }
        else 
        {
            playerChase = true;
            isAllucination = true;
        }


    }

    // Update is called once per frame
    void Update()
    {
        if(isAllucination == true)
        {
            playerChase = true; 
        }


            if (playerChase == true)
        {
            alertAudioVarSource.ReproducirSonidoConVariacion();
            while (playerChase == true)
                { 
                    
                    //chase player script
                    Vector3 playerPosition = player.transform.position;


                    Debug.DrawRay(playerPosition, Vector3.up, Color.blue, 1.0f); //debug para ver
                    navAgent.SetDestination(playerPosition);
                }

                animator.SetBool("Running", true);
           
        } else 
        { animator.SetBool("Running", false); }

        if (currentLife <= 0)
        {
            animator.SetBool("isDead", true);
            Destroy(gameObject, 1);
        }

    }

    void OnAnimatorMove()
    {
        if (animator.GetBool("Running"))
        {
            navAgent.speed = (animator.deltaPosition / Time.deltaTime).magnitude;
        }
    }
}
