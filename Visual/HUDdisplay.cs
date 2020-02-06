using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDdisplay : MonoBehaviour
{
    [Header("Game Data")]
    [SerializeField] Weapon weaponData;
    [SerializeField] WeaponHandler weaponHandler;

    [Header("HUDElements")]
    [SerializeField] TextMeshProUGUI tmpObj = null;

    private void Update()
    {
        tmpObj.text = weaponData.getAmmo();
    }

    public void changeWeapon()
    {
        weaponData = weaponHandler.getSelectedWeapon();
    }
}
