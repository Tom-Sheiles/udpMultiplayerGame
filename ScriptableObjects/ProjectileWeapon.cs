using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewProjectileWeapon", menuName = "ScriptObjects/Weapons/Projectile/instantProjectile")]
public class ProjectileWeapon : Weapon
{
    public override string getAmmo()
    {
        throw new System.NotImplementedException();
    }

    public override GameObject initialize(Transform transform, int id)
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerator reloadCoroutine()
    {
        throw new System.NotImplementedException();
    }
}
