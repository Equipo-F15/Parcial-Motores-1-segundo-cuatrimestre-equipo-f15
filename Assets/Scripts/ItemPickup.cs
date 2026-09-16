using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    // Este método detecta cuando otro objeto entra en su espacio
    void OnTriggerEnter(Collider other)
    {
        // Busca si el objeto que lo chocó tiene el script ItemsManager
        ItemsManager inventory = other.GetComponent<ItemsManager>();

        if (inventory != null)
        {
            inventory.RecogerItemDesdeSuelo();

            Destroy(gameObject);
        }
    }
}