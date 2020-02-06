using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newMultirayweapon", menuName = "ScriptObjects/Weapons/Hitscan/Multi Ray Weapon")]
public class MultiRaycast : RaycastWeapon
{

    [Header("Weapon Stats")]
    [Range(1, 20)]
    public int numberOfRays = 1;

    public int damagePerShot;
    public float bulletSpread;

    private bool hasShot = false;

   public List<RaycastHit> multiRaycast(Transform transform)
    {

        RaycastHit hit;
        List<RaycastHit> hitObjects = new List<RaycastHit>();

        if (!canRaycast() || isReloading)
        {
            return hitObjects;
        }

        animator.ResetTrigger("shoot");
        animator.SetTrigger("shoot");

        Debug.DrawRay(transform.position, transform.forward * shotDistance, Color.red, fireRate);
        for(int ray = 0; ray < numberOfRays; ray++)
        {
            Vector3 rayDirection = Vector3.forward;
            float spread = Random.Range(0f, bulletSpread);
            float angle = Random.Range(0f, 360f);
            rayDirection = Quaternion.AngleAxis(spread, Vector3.up) * rayDirection;
            rayDirection = Quaternion.AngleAxis(angle, Vector3.forward) * rayDirection;
            rayDirection = transform.rotation * rayDirection;

            Debug.DrawRay(transform.position, rayDirection * shotDistance, Color.blue, fireRate);

            if(Physics.Raycast(transform.position, rayDirection, out hit, shotDistance))
            {
                hitObjects.Add(hit);
            }
        }
        reduceAmmo();
        return hitObjects;
    }
}
