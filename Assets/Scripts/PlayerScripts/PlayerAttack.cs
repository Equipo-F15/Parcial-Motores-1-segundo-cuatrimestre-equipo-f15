using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] float attackRange;
    [SerializeField] private Transform rayOrigin;
    private LifeScript lifeScript;

    [SerializeField] private ItemsManager itemsManager; // Referencia al inventario del jugador.
    [SerializeField] private ItemData knifeData; // Referencia al ItemData del cuchillo.

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

            RaycastHit hit;
            bool huboImpacto = Physics.Raycast(rayOrigin.position, rayOrigin.forward, out hit, attackRange);
            if (huboImpacto)
            {
                lifeScript = hit.collider.GetComponentInParent<LifeScript>();

                if (lifeScript != null)
                {
                    lifeScript.TakesDamage(1); // Le quita 1 punto de vida al enemigo.

                    itemsManager.ReduceDurability(knifeData, 1); // Descuenta 1 punto de durabilidad del cuchillo en el inventario.
                }
                else
                {
                    Debug.Log("El objeto NO tiene IInteractable");
                }
            }
        }
    }
}