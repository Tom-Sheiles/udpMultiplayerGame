using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDdisplay : MonoBehaviour
{
    [Header("Game Data")]
    [SerializeField] Weapon weaponData = null;
    [SerializeField] WeaponHandler weaponHandler = null;
    [SerializeField] PlayerMovement playerMovement = null;
    [SerializeField] PlayerHealth playerHealth = null;

    [Header("HUDElements")]
    [SerializeField] TextMeshProUGUI tmpObj = null;
    [SerializeField] Slider sprintSlider = null;
    [SerializeField] GameObject sprintValue = null;
    [SerializeField] Slider healthBar = null;
    [SerializeField] TextMeshProUGUI healthtext = null;
    [SerializeField] GameObject[] hudElements;

    private void Start()
    {
        sprintSlider.maxValue = playerMovement.sprintTime;
        sprintSlider.minValue = 0;
        sprintSlider.value = playerMovement.sprintTime;
    }

    private void Update()
    {
        tmpObj.text = weaponData.getAmmo();

        sprintSlider.value = playerMovement.currentSprintRemaining;

        if(playerMovement.currentSprintRemaining <= 0)
        {
            sprintValue.SetActive(false);
        }
        else
        {
            sprintValue.SetActive(true);
        }

        healthBar.value = playerHealth.currentHealth;
        healthtext.text = playerHealth.currentHealth.ToString();
    }

    public void changeWeapon()
    {
        weaponData = weaponHandler.getSelectedWeapon();
    }

    public void playerDied()
    {
        foreach(GameObject element in hudElements)
        {
            element.SetActive(false);
        }
    }

    public void playerRespawn()
    {
        foreach(GameObject element in hudElements)
        {
            element.SetActive(true);
        }
    }
}
