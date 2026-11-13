using UnityEngine;
using UnityEngine.UI;

public class SanityBarUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private float maxSanity = 100f; // same value as sanityMax in SanityScript

    private SanityScript sanityScript;

    private void Start()
    {
        sanityScript = FindFirstObjectByType<SanityScript>();

        if (sanityScript == null)
        {
            Debug.LogError("SanityBarUI: no SanityScript found in the scene", this);
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