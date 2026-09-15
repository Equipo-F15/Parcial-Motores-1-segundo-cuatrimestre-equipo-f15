using UnityEngine;

public class DoorExit : MonoBehaviour, IInteractable
{
    [SerializeField] private float openAngle = 90f;

    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    // Guarda la rotación inicial y calcula la rotación de apertura
    private void Start()
    {
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(0, openAngle, 0) * closedRotation;
    }

    // // Se ejecuta cuando el jugador interactúa con la puerta
    public void Interact()
    {
        if (!KeyPickup.hasKey)
        {
            Debug.Log("You need the key!"); //Te avisa en la consola que necesitas la llave para abrir la puerta
            return;
        }
        
        // Cambia el estado de la puerta entre abierta y cerrada
        isOpen = !isOpen;

        if (isOpen)
        {
            transform.rotation = openRotation;
        }
        else
        {
            transform.rotation = closedRotation;
        }
    }
}