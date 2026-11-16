using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] float attackRange;
    [SerializeField] private Transform rayOrigin;
    [SerializeField] private KnifeBarUI knifeBar;
    private LifeScript lifeScript;

    void Start()
    {
        if (knifeBar == null)
        {
            knifeBar = FindFirstObjectByType<KnifeBarUI>();
        }
    }

    void Update()
    {
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