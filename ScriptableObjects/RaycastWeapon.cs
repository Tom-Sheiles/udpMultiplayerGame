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

    public int maxReserve;
    public int currentReserve;

    [Header("Weapon Stats")]
    [SerializeField] public int Damage;
    public float shotDistance;
    public float fireRate;
    protected float shotTimer;
    public float reloadTime;

    
    [HideInInspector] public bool hasShotWhenReload = false;


    public override GameObject initialize(Transform transform, int id)
    {
        shotTimer = 0f;
        currentClipSize = maxClipSize;
        isReloading = false;

        if(weaponModel != null)
        {
            var viewModel = Instantiate(weaponModel, transform.position, transform.rotation);
            viewModel.transform.parent = transform.gameObject.transform;

            viewModel.transform.localScale = weaponModel.transform.localScale;
            viewModel.GetComponentInChildren<viewModelColor>().changeColor(id);
            return viewModel;
        }
        return new GameObject();
        
        
    }

    public RaycastHit instanceRay(Transform position, Transform bulletOrigin, NetworkInstantiate networkInstantiate, int id)
    {
        RaycastHit hit = new RaycastHit();
        if (!canRaycast())
        {
            return hit;
        }

        animator.ResetTrigger("shoot");
        animator.SetTrigger("shoot");

        reduceAmmo();
        animator.SetInteger("ammo", currentClipSize);
        Instantiate(bulletVFX, bulletOrigin.position, bulletOrigin.rotation);
        networkInstantiate.instantiate(NetworkInstantiate.prefabNames.bullet, bulletOrigin.position, bulletOrigin.rotation, id);
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
                    if (hasShotWhenReload == true)
                    {
                        hasShotWhenReload = false;
                        isReloading = false;
                        yield break;
                    }
                    yield return new WaitForSeconds(reloadTime);
                    currentClipSize++;
                }
            }
        }


        hasShotWhenReload = false;
        isReloading = false;
        animator.SetInteger("ammo", currentClipSize);
    }

    public override string getAmmo()
    {
        return currentClipSize.ToString();
    }
}

