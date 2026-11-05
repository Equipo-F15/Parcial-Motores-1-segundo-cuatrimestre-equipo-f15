using UnityEngine;

public class AmbienceZone : MonoBehaviour
{
    [SerializeField] private AmbienceController ambience;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) ambience.SetInside(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) ambience.SetInside(false);
    }
}