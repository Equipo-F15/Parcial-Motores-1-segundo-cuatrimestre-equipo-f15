using UnityEngine;

public class Door : MonoBehaviour, IInteractable //script que se le pone a la puerta para que se pueda abrir y cerrar
{
    [SerializeField] private float openAngle = 90f; // angulo de apertura de la puerta

    private bool isOpen = false; // nos avisa si la puerta esta abierta o cerrada
    private Quaternion closedRotation; 
    private Quaternion openRotation;

    private void Start() // se ejecuta al iniciar el juego para saber si esta abierta o cerrada la puerta y asi poder abrirla o cerrarla
    {
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(0, openAngle, 0) * closedRotation; // calcula el angulo de apertura de la puerta
    }

    public void Interact() // llama a la interfaz interactable para abrir o cerrar la puerta
    {
        isOpen = !isOpen; // cambia el estado de la puerta, si estaba cerrada abre y si estaba abierta cierra

        if (isOpen)
        {
            transform.rotation = openRotation; // cambia la rotacion de la puerta a la rotacion de apertura
        }
        else
        {
            transform.rotation = closedRotation; // cambia la rotacion de la puerta a la rotacion de cierre
        }
    }
}