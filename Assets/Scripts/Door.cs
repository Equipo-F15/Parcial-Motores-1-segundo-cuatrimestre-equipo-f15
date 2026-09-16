using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private float openAngle = 90f;

    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    private void Start()
    {
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(0, openAngle, 0) * closedRotation;
    }

    public void Interact()
    {
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