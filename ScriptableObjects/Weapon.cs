using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : ScriptableObject
{
    [Header("Display Options")]
    public GameObject weaponModel;

    public abstract void initialize(Transform transform);
}

