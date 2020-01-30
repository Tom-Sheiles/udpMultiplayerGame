using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrictFollowObject : MonoBehaviour
{

    [SerializeField] public Transform target;
    [SerializeField] Vector3 offset;

    void LateUpdate()
    {
        Vector3 position = new Vector3(target.position.x + offset.x, target.position.y + offset.y, target.position.z + offset.z);
        transform.position = position;
    }
}
