using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothObjectMove : MonoBehaviour
{

    [SerializeField] private Transform target;
    [SerializeField] private float time;

    void Start()
    {
            
    }

    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.transform.position, time);
    }
}
