using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    // Objeto que representa este GameObject del escenario.
    [SerializeField] private ItemData itemData;
    [Header("Sonido de recolección")]
    [SerializeField] private AudioClip sonidoRecoger; // Sonido que se reproduce al recoger el objeto.

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

        if (collected)
        {
            // Reproduce el sonido desde el jugador para que no se corte al destruir el objeto.
            if (sonidoRecoger != null)
            {
                // Busca el AudioSource del jugador que recogió el objeto.
                AudioSource audioJugador = other.GetComponent<AudioSource>();

                // Reproduce el sonido desde el jugador, sin depender de la distancia.
                if (audioJugador != null)
                {
                    audioJugador.PlayOneShot(sonidoRecoger);
                }
            }

            // Elimina el objeto del escenario después de recogerlo.
            Destroy(gameObject);
        }
    }
}
