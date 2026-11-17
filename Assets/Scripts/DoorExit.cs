using UnityEngine;

public class DoorExit : MonoBehaviour, IInteractable
{
    [SerializeField] private float openAngle = 90f;

    // Referencia al inventario del jugador.
    [SerializeField] private ItemsManager itemsManager;
    // Referencia al ItemData de la llave necesaria para escapar.
    [SerializeField] private ItemData requiredKey;

    private bool isOpen = false;
    public bool IsOpen // Permite consultar desde otros scripts si la puerta está abierta. No permite modificar su estado desde afuera.
    {
        get { return isOpen; }
    }

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
        // Comprobacion que el inventario y la llave estén configurados.
        if (itemsManager == null || requiredKey == null)
        {
            Debug.LogError("Faltan referencias en DoorExit.");
            return;
        }

        // Comprobacion si el jugador tiene la llave en su inventario.
        if (!itemsManager.HasItem(requiredKey))
        {
            Debug.Log("Necesitás la llave para abrir esta puerta.");
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