using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] Weapon weapon;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "PlayerObject")
        {
            WeaponHandler handler = other.GetComponent<WeaponHandler>();
            if (handler.numberOfWeapons < 2)
            {
                handler.Pickup(weapon);
                Destroy(gameObject);
            }
        }
    }
}