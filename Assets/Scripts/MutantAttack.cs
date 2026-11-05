using UnityEngine;
using System.Collections;

public class MutantAttack : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private HealthBar healthBar;

    float distanceToPlayer;
    public float distanceToAttack = 2f;
    public float attackDelay = 1f;
    public float attackAngle = 45f;
    [SerializeField] private float damage = 20f;

    private Animator animator;
    private bool canAttack = true;

    void Start()
    {
        animator = GetComponent<Animator>();
    
        healthBar = FindFirstObjectByType<HealthBar>();
    }

    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= distanceToAttack && canAttack)
        {
            Vector3 side1 = player.transform.position - transform.position;
            Vector3 side2 = transform.forward;
            float angle = Vector3.SignedAngle(side1, side2, Vector3.up);

            if (angle < attackAngle && angle > -1 * attackAngle)
            {
                StartCoroutine(Attack());
            }
        }
    }

    IEnumerator Attack()
    {
        canAttack = false;
        animator.SetBool("IsAttacking", true);
        yield return new WaitForSeconds(attackDelay);
        healthBar.TakeDamage(damage);
        animator.SetBool("IsAttacking", false);
        yield return new WaitForSeconds(1f);
        canAttack = true;
    }
}