using UnityEngine;

public class InteractionRaycaster : MonoBehaviour //se pone al jugador 
{ 
    [SerializeField] private float interactionRange = 3f; // es el rango que tiene el jugador para interactuar con el objeto
    [SerializeField] private Transform rayOrigin; // donde comienza la interacción
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // se presiona la tecla E para interactuar y se pregunta en cada frame si se presionó
        {
            Debug.Log("Posición del rayOrigin: " + rayOrigin.position);
            Debug.Log("Dirección (forward): " + rayOrigin.forward);
            TryInteract();
        }
    }

    private void TryInteract() 
        RaycastHit hit; // se activa cuando se presiona la tecla E y se hace un raycast para ver si hay un objeto interactuable en frente del jugador, devuelve hit
        bool huboImpacto = Physics.Raycast(rayOrigin.position, rayOrigin.forward, out hit, interactionRange);

        Debug.Log("Hubo impacto: " + huboImpacto);

        if (huboImpacto)
        {
            Debug.Log("Le pegó a: " + hit.collider.name);

            IInteractable interactuable = hit.collider.GetComponentInParent<IInteractable>(); // buscamos si hubo impacto y se devuelve el componente IInteractable del objeto que fue impactado y sus hijos porque suele tenerlo la visagra, que seria el padre de la puerta, con la que se interactua, si no tiene devuelve null

            if (interactuable != null) // si el objeto tiene IInteractable, se llama al método Interact() del objeto
            {
                Debug.Log("Tiene IInteractable, llamando Interact()");
                interactuable.Interact();
            }
            else
            {
                Debug.Log("El objeto NO tiene IInteractable");
            }
        }
    }
}