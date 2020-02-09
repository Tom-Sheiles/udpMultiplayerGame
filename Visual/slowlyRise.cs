using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slowlyRise : MonoBehaviour
{

    [SerializeField] float riseRate;

    private void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + (riseRate * Time.deltaTime), transform.position.z);
    }
}
