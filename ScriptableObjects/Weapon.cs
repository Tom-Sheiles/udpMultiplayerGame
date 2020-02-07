using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : ScriptableObject
{
    [Header("Display Options")]
    public GameObject weaponModel;
    public Animator animator;
    public GameObject bulletVFX;
    [HideInInspector] public bool isReloading = false;
    public int currentClipSize;

    public bool isAutomatic = false;

    public abstract GameObject initialize(Transform transform);
    public abstract IEnumerator reloadCoroutine();
    public abstract string getAmmo();

    public void setAnimator(Animator animator)
    {
        this.animator = animator;
        this.animator.SetInteger("ammo", currentClipSize);
    }
}

