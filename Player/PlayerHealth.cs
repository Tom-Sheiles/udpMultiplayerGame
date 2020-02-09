using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public int maxHealth;
    public int currentHealth;

    [SerializeField] ClientSceneManager sceneManager = null;
    [SerializeField] float RagdollSpawnTime = 0.3f;
    [SerializeField] float deathCamHeight = 1.5f;
    [SerializeField] float deathCamXOffset = 4f;
    GameObject playerCamera;
    [SerializeField] GameObject deathCamPrefab;
    [SerializeField] GameEvent playerDied;
    PlayerMovement playerMovement;

    GameObject deathCam;
    
    void Start()
    {
        currentHealth = maxHealth;
        playerMovement = GetComponent<PlayerMovement>();
        playerCamera = Camera.main.gameObject;
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
        playerCamera.SetActive(false);

        var deathPoint = transform.position;

        var camLocation = deathPoint;
        camLocation.x += deathCamXOffset;
        camLocation.y += deathCamHeight;

        deathCam = Instantiate(deathCamPrefab, camLocation, transform.rotation);

        deathCam.transform.LookAt(gameObject.transform);

        playerMovement.changePosition(new Vector3(1,1,1));

        StartCoroutine("ragdollCoroutine", deathPoint); 
        
        currentHealth = maxHealth;

        playerDied.Raise();

    }

    public void OnRespawn()
    {
        Destroy(deathCam);
        var nextSpawn = sceneManager.getnextspawn();
        playerMovement.changePosition(nextSpawn);
        playerCamera.SetActive(true);
    }

    private IEnumerator ragdollCoroutine(Vector3 deathpoint)
    {
        yield return new WaitForSeconds(RagdollSpawnTime);
        sceneManager.sendRagdollSpawn(deathpoint);
    }
}
