using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookAtMainCamera : MonoBehaviour
{
    Transform cameraTransform;

    void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        transform.LookAt(cameraTransform);
    }
}
