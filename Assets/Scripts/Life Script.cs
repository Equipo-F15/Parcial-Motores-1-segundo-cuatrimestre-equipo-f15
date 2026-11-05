using UnityEngine;

public class LifeScript : MonoBehaviour
{
    public int maxLife;
    public int currentLife;
    public bool isDead;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentLife = maxLife;
        isDead = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakesDamage( int damage)
    {
        currentLife = - damage;
        if (currentLife <= 0)
        { isDead = true; }
    }

}
