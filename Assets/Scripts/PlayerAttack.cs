using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;

    [SerializeField] float attackRange;
    [SerializeField] private Transform rayOrigin;
    [SerializeField] private KnifeBarUI knifeBar;
    private LifeScript lifeScript;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        
        if (knifeBar == null)
        {
            knifeBar = FindFirstObjectByType<KnifeBarUI>();
        }
    }

    void Update()
    {
        animator.SetBool("IsAttacking", false);
        if (Input.GetMouseButtonDown(0))
        {
            
            if (knifeBar != null && !knifeBar.HasUses())
            {
                Debug.Log("The knife is broken, you cannot use it anymore");
                return;
            }

            RaycastHit hit;
            bool huboImpacto = Physics.Raycast(rayOrigin.position, rayOrigin.forward, out hit, attackRange);
            if (huboImpacto)
            {
                animator.SetBool("IsAttacking", true);
                lifeScript = hit.collider.GetComponentInParent<LifeScript>();

                if (lifeScript != null)
                {
                    lifeScript.TakesDamage(1);
                    if (knifeBar != null) knifeBar.UseKnife();
                }
                else
                {
                    Debug.Log("El objeto NO tiene IInteractable");
                }
            }
        }
    }
}