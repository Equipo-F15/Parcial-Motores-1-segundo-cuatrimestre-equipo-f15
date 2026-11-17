using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] float attackRange;
    [SerializeField] private Transform rayOrigin;
    [SerializeField] private KnifeBarUI knifeBar;
    private LifeScript lifeScript;
    Animator animator;


    [SerializeField] private ItemsManager itemsManager; // Referencia al inventario del jugador.
    [SerializeField] private ItemData knifeData; // Referencia al ItemData del cuchillo.

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
        
        if (Input.GetMouseButtonDown(0))
        {
            // Comprobacion que las referencias del inventario estén configuradas.
            if (itemsManager == null || knifeData == null)
            {
                Debug.LogError("Faltan referencias del inventario o del cuchillo en PlayerAttack.");
                return;
            }

            // Busca el cuchillo dentro del inventario.
            InventoryItem knife = itemsManager.GetItems().Find(item => item.itemData == knifeData);

            // Si no tenemos el cuchillo, no podemos atacar.
            if (knife == null)
            {
                Debug.Log("No tenés un cuchillo en el inventario.");
                return;
            }

            // Si el cuchillo está roto, tampoco podemos atacar.
            if (knife.currentDurability <= 0)
            {
                Debug.Log("El cuchillo está roto y necesita repararse.");
                return;
            }

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
                    animator.SetBool("IsAttacking", true);
                    lifeScript.TakesDamage(1);
                    itemsManager.ReduceDurability(knifeData, 1);
                    if (knifeBar != null) knifeBar.UseKnife();
                }
                else
                {
                    Debug.Log("El objeto NO tiene IInteractable");
                }
            }
        }
    }

        public void StopAttack()
        {
            animator.SetBool("IsAttacking", false);
        }
    }