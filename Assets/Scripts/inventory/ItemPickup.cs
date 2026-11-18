using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    // Objeto que representa este GameObject del escenario.
    [SerializeField] private ItemData itemData;

    // Se ejecuta cuando otro Collider entra en el Trigger de este objeto.
    private void OnTriggerEnter(Collider other)
    {
        // Busca el inventario en el objeto que entró.
        ItemsManager inventory = other.GetComponent<ItemsManager>();

        // Si el objeto que entró no tiene inventario, no hace nada.
        if (inventory == null)
        {
            return;
        }

        // Comprobar que haya asignado un ItemData.
        if (itemData == null)
        {
            Debug.LogWarning("Este objeto no tiene ItemData asignado.");
            return;
        }

        // Intenta agregar el objeto al inventario.
        bool collected = inventory.AddItem(itemData);

        // Solamente destruye el objeto del escenario cuando se agregó correctamente al inventario.
        if (collected)
        {
            Destroy(gameObject);
        }
    }
}
