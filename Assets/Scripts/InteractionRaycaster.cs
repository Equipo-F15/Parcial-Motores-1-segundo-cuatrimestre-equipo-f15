using UnityEngine;

public class InteractionRaycaster : MonoBehaviour
{
    [SerializeField] private float interactionRange = 3f;
    [SerializeField] private Transform rayOrigin;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    private void TryInteract()
    {
        RaycastHit hit;
        bool huboImpacto = Physics.Raycast(rayOrigin.position, rayOrigin.forward, out hit, interactionRange);

        if (huboImpacto)
        {
            IInteractable interactuable = hit.collider.GetComponent<IInteractable>();

            if (interactuable != null)
            {
                interactuable.Interact();
            }
        }
    }
}