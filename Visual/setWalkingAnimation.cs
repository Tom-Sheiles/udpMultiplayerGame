using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setWalkingAnimation : MonoBehaviour
{
    Vector3 PrevPos;
    Vector3 NewPos;
    Vector3 ObjVelocity;
    Animator animator;

    private void Start()
    {
        PrevPos = transform.position;
        NewPos = transform.position;

        animator = GetComponentInChildren<Animator>();
    }

    private void FixedUpdate()
    {
        NewPos = transform.position;  // each frame track the new position
        ObjVelocity = (NewPos - PrevPos) / Time.fixedDeltaTime;  // velocity = dist/time
        PrevPos = NewPos;  // update position for next frame calculation

        animator.SetFloat("movespeed", ObjVelocity.magnitude);
    }
}
