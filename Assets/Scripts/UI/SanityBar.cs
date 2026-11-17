using UnityEngine;
using UnityEngine.UI;

public class SanityBarUI : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Image fillImage;
    [SerializeField] private float maxSanity = 240f; 

    private SanityScript sanityScript;

    private void Start()
    {
        
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning("SanityBarUI: no object with tag Player found", this);
            enabled = false;
            return;
        }

        sanityScript = player.GetComponent<SanityScript>();

        if (sanityScript == null)
        {
            Debug.LogWarning("SanityBarUI: the player has no SanityScript", this);
            enabled = false;
            return;
        }

        if (fillImage == null)
        {
            Debug.LogError("SanityBarUI: Fill Image is not assigned", this);
            enabled = false;
        }
    }

    private void Update()
    {
        float value = Mathf.Clamp(sanityScript.sanity, 0f, maxSanity);
        fillImage.fillAmount = value / maxSanity;
    }
}