using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeZone : MonoBehaviour
{
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("EscapeZone touched by: " + other.name + " | tag: " + other.tag);

        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            if (!KeyPickup.hasKey)
            {
                Debug.Log("You need the key to escape");
                return;
            }

            triggered = true;
            SceneManager.LoadScene("Victory");
        }
    }
}