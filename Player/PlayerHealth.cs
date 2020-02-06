using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public int maxHealth;
    private int currentHealth;
    
    void Start()
    {
        currentHealth = maxHealth;
    }


    void Update()
    {
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            respawnPlayer();
        }
    }

    public void takeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
    }

    public void gainHealth(int healAmount)
    {
        currentHealth += healAmount;
    }

    public void respawnPlayer()
    {

    }
}
