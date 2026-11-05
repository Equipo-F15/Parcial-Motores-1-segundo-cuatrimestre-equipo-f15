
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthBar : MonoBehaviour
{
    public Image healthBar;
    public float maxHealth = 100f;
    public float currentHealth;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.fillAmount = 1f;
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;
        healthBar.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0f) Die();
    }

    private void Die()
    {
        isDead = true;
        SceneManager.LoadScene("GameOver");
    }
    void Update()
{
    // Si apretás la H, es como si te pegaran
    if (Input.GetKeyDown(KeyCode.H))
    {
        TakeDamage(20);
    }
}
}