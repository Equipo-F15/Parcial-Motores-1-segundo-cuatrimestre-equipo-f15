using UnityEngine;
using UnityEngine.UI;

public class KnifeBarUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private float costPerUse = 0.25f; // 25% per use

    private float current = 1f;

    private void Start()
    {
        if (fillImage == null)
        {
            Debug.LogError("KnifeBarUI: Fill Image is not assigned", this);
            return;
        }

        UpdateBar();
    }

    public bool HasUses()
    {
        return current > 0.001f;
    }

    public bool UseKnife()
    {
        if (!HasUses() || fillImage == null)
        {
            return false;
        }

        current = Mathf.Clamp01(current - costPerUse);
        UpdateBar();
        return true;
    }

    private void UpdateBar()
    {
        fillImage.fillAmount = current;
    }
}