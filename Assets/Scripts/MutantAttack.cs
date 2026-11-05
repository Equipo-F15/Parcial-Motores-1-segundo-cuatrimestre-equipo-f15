using UnityEngine;

public class MutantAttack : MonoBehaviour
{

    [SerializeField] private GameObject player;
    float distanceToPlayer;
    public float distanceToAttack;
    public float attackDelay;
    public float attackAngle;
   [SerializeField] private float damage;

    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= distanceToAttack)
        {
            Vector3 side1 = player.transform.position - transform.position;
            Vector3 side2 = transform.forward;
            float angle = Vector3.SignedAngle(side1, side2, Vector3.up);

            if (angle < attackAngle && angle > -1 * attackAngle)
            {
                animator.SetBool("IsAttacking",true);
                //player.TakeDamage(damage);
                WaitForSeconds wait = new WaitForSeconds(attackDelay);
                Debug.Log("Player in angle");

            } 
        }
    }
}
