using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteAfterTime : MonoBehaviour
{
    [SerializeField] float seconds;

    void Update()
    {
        if(seconds <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            seconds -= Time.deltaTime;
        }
    }
}
