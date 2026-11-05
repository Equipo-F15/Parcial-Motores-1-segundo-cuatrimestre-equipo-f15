using UnityEngine;

public class LifeScript : MonoBehaviour
{
    public int maxLife;
    public int currentLife;
    public bool IsDead;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentLife = maxLife;
        IsDead = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakesDamage(int damage)
{
    currentLife -= damage;
    if (currentLife <= 0)
    {
        IsDead = true;
    }
}

}
