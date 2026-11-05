using UnityEngine;
using UnityEngine.UI;

public class KnifeBarUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private float costPerUse = 0.25f; // 25%

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

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.H)) UseKnife();
    }

    public bool HasUses()
    {
        return current > 0.001f;
    }

    
    public void UseKnife()
    {
        if (!HasUses() || fillImage == null) return;

        current = Mathf.Clamp01(current - costPerUse);
        UpdateBar();
    }

    private void UpdateBar()
    {
        fillImage.fillAmount = current;
    }
}