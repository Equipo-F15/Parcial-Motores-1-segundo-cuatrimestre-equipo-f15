using UnityEngine;

public class InteractionRaycaster : MonoBehaviour
{
    [SerializeField] private float interactionRange = 3f;
    [SerializeField] private Transform rayOrigin;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Posición del rayOrigin: " + rayOrigin.position);
            Debug.Log("Dirección (forward): " + rayOrigin.forward);
            TryInteract();
        }
    }

    private void TryInteract()
    {
        RaycastHit hit;
        bool huboImpacto = Physics.Raycast(rayOrigin.position, rayOrigin.forward, out hit, interactionRange);

        Debug.Log("Hubo impacto: " + huboImpacto);

        if (huboImpacto)
        {
            Debug.Log("Le pegó a: " + hit.collider.name);

            IInteractable interactuable = hit.collider.GetComponent<IInteractable>();

            if (interactuable != null)
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