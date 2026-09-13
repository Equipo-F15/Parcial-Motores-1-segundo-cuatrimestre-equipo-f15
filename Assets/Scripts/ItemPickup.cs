using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    // Este método detecta cuando otro objeto entra en su espacio
    void OnTriggerEnter(Collider other)
    {
        // Busca si el objeto que lo chocó tiene el script ItemsManager
        ItemsManager inventor = other.GetComponent<ItemsManager>();

        if (inventor != null)
        {
            inventor.RecogerItemDesdeSuelo();

            Destroy(gameObject);
        }
    }
}