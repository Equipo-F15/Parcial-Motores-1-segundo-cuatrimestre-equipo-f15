using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeZone : MonoBehaviour
{
    [SerializeField] private ItemData keyItem; // the ItemData asset of the key

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (!other.CompareTag("Player")) return;

        ItemsManager inventory = other.GetComponent<ItemsManager>();

        if (inventory == null || keyItem == null || !inventory.HasItem(keyItem))
        {
            Debug.Log("You need the key to escape");
            return;
        }

        triggered = true;
        SceneManager.LoadScene("Victory");
    }
}