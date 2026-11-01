using UnityEngine;
using UnityEngine.UI;

public class StaminaBarTest : MonoBehaviour
{
    public Slider slider;
    public float maxStamina = 100f;
    public float currentStamina;
    public float drainRate = 20f;
    public float regenRate = 15f;

    void Start()
    {
        currentStamina = maxStamina;
        slider.maxValue = maxStamina;
        slider.value = currentStamina;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentStamina -= drainRate * Time.deltaTime;
        }
        else
        {
            currentStamina += regenRate * Time.deltaTime;
        }

        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        slider.value = currentStamina;
    }
}