using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : ScriptableObject
{
    [Header("Display Options")]
    public GameObject weaponModel;
    public Animator animator;
    [HideInInspector] public bool isReloading = false;

    public bool isAutomatic = false;

    public abstract GameObject initialize(Transform transform);
    public abstract IEnumerator reloadCoroutine();
    public abstract string getAmmo();

    public void setAnimator(Animator animator)
    {
        this.animator = animator;
    }
}

