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
        
    }

    public void takeDamage(int damageAmount)
    {
        Debug.Log(damageAmount);
    }
}
