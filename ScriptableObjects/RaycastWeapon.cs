using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRaycastWeapon",menuName = "ScriptObjects/Weapons/Hitscan/singleRaycast")]
public class RaycastWeapon : Weapon
{
    [Header("Ammo Stats")]
    public bool noReload = false;
    public int maxClipSize;
    public int currentClipSize;

    public int maxReserve;
    public int currentReserve;

    [Header("Weapon Stats")]
    [SerializeField] private float Damage;
    public float shotDistance;
    public float fireRate;
    protected float shotTimer;


    public override void initialize(Transform transform)
    {
        shotTimer = 0f;
        var viewModel = Instantiate(weaponModel, transform.position, transform.rotation);
        viewModel.transform.parent = transform.gameObject.transform;
    }

    public RaycastHit instanceRay(Transform position)
    {
        RaycastHit hit = new RaycastHit();
        if (!canRaycast())
        {
            return hit;
        }
        
        Debug.Log("ray");

        if (Physics.Raycast(position.transform.position, position.forward, out hit, shotDistance))
        {
            //Debug.Log(hit.transform.gameObject.name);
            Debug.DrawRay(position.transform.position, position.forward * shotDistance, Color.red, 10);
            return hit;
        }
        else
        {
            Debug.DrawRay(position.transform.position, position.forward * shotDistance, Color.red, 10);
        }
        return hit;
    }

    protected bool canRaycast()
    {
        if(Time.time > shotTimer)
        {
            shotTimer = Time.time + fireRate;
            return true;
        }
        else
        {
            return false;
        }
    }
}

