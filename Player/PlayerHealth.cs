using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public int maxHealth;
    public int currentHealth;

    [SerializeField] ClientSceneManager sceneManager = null;
    [SerializeField] float RagdollSpawnTime = 0.3f;
    PlayerMovement playerMovement;
    
    void Start()
    {
        currentHealth = maxHealth;
        playerMovement = GetComponent<PlayerMovement>();
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

        var deathPoint = transform.position;
        playerMovement.changePosition(new Vector3(725.032f, 70, 244.5742f));

        StartCoroutine("ragdollCoroutine", deathPoint); 
        
        currentHealth = maxHealth;
        
    }

    private IEnumerator ragdollCoroutine(Vector3 deathpoint)
    {
        yield return new WaitForSeconds(RagdollSpawnTime);
        sceneManager.sendRagdollSpawn(deathpoint);
    }
}
