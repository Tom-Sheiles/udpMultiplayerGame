using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] Weapon[] weapons = new Weapon[2];
    [SerializeField] Transform raycastOrigin;
    [SerializeField] Transform viewModelPosition;
    private int selectedWeapon = 0;

    private void Start()
    {
        foreach(Weapon weapon in weapons)
        {
            if(weapon != null)
            weapon.initialize(viewModelPosition);
        }
    }

    private void Update()
    {
        
        if (Input.GetButtonDown("Fire1"))
        {
            handleWeapon();
        }
    }

    private void handleWeapon()
    {
        if (weapons[selectedWeapon].GetType() ==  typeof(RaycastWeapon))
        {
            //Debug.Log("Selected Weapon is " + weapons[selectedWeapon].name);
            ((RaycastWeapon)weapons[selectedWeapon]).instanceRay(raycastOrigin);
        }

        if(weapons[selectedWeapon].GetType() == typeof(MultiRaycast))
        {
            ((MultiRaycast)weapons[selectedWeapon]).multiRaycast(raycastOrigin);
        }
    }
}
