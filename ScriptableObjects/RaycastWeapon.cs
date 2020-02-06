using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRaycastWeapon",menuName = "ScriptObjects/Weapons/Hitscan/singleRaycast")]
public class RaycastWeapon : Weapon
{
    [Header("Ammo Stats")]
    public bool infiniteAmmo = false;
    public bool hasMagazine = true;
    public int maxClipSize;
    public int currentClipSize;

    public int maxReserve;
    public int currentReserve;

    [Header("Weapon Stats")]
    [SerializeField] private float Damage;
    public float shotDistance;
    public float fireRate;
    protected float shotTimer;
    public float reloadTime;

    
    [HideInInspector] public bool hasShotWhenReload = false;


    public override GameObject initialize(Transform transform)
    {
        shotTimer = 0f;
        currentClipSize = maxClipSize;
        isReloading = false;

        if(weaponModel != null)
        {
            var viewModel = Instantiate(weaponModel, transform.position, transform.rotation);
            viewModel.transform.parent = transform.gameObject.transform;
            viewModel.transform.localScale = new Vector3(1, 1, 1);
            return viewModel;
        }
        return new GameObject();
        
        
    }

    public RaycastHit instanceRay(Transform position)
    {
        RaycastHit hit = new RaycastHit();
        if (!canRaycast())
        {
            return hit;
        }

        reduceAmmo();
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

    protected void reduceAmmo()
    {
        currentClipSize -= 1;
    }

    protected bool canRaycast()
    {
        if(Time.time < shotTimer)
        {
            return false;
            
        }

        if(currentClipSize <= 0)
        {
          
            return false;
        }

        if (isReloading)
        {
            return false;
        }

        shotTimer = Time.time + fireRate;
        return true;

    }

    public override IEnumerator reloadCoroutine()
    {

        hasShotWhenReload = false;

        if (isReloading || currentClipSize == maxClipSize)
        {
            yield break;
        }
        isReloading = true;

        if (infiniteAmmo)
        {
            if (hasMagazine)
            {
                yield return new WaitForSeconds(reloadTime);
                currentClipSize = maxClipSize;
            }
            else
            {
                for(int clip = currentClipSize; clip < maxClipSize; clip++)
                {
                    yield return new WaitForSeconds(reloadTime);

                    if (hasShotWhenReload == true)
                    {
                        hasShotWhenReload = false;
                        isReloading = false;
                        yield break;
                    }
                    currentClipSize++;
                }
            }
        }


        hasShotWhenReload = false;
        isReloading = false;
    }

    public override string getAmmo()
    {
        return currentClipSize.ToString();
    }
}

