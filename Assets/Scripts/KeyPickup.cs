using UnityEngine;

public class KeyPickup : MonoBehaviour
{

    // Si el jugador tiene la llave
    // false = no tiene la llave
    // true = tiene la llave
    public static bool hasKey = false;

    //Se ejecuta cuando el jugador entra en el trigger de la llave
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))          // Si el objeto que entra en el trigger tiene la etiqueta "Player"
        {
            hasKey = true;

            Debug.Log("Key picked up!");        //Mensaje en la consola para avisar que el jugador tiene la llave

            Destroy(gameObject);
        }
    }
}