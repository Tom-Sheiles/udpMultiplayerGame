using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class viewModelColor : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer[] arms;
    [SerializeField] Material[] mats;

    public void changeColor(int colorId)
    {
        foreach(SkinnedMeshRenderer mesh in arms)
        {
            mesh.material = mats[colorId];
        }
    }
}
