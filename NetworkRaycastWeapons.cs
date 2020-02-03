using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkRaycastWeapons : MonoBehaviour
{

    private GameObject camera;

    [SerializeField] ClientSceneManager sceneManager;
    [SerializeField] float projectileMaxDistance;
    public int damage = 10;

    void Start()
    {
        camera = Camera.main.gameObject;
    }


    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            fireRay();
        }
    }

    private void fireRay()
    {
        RaycastHit objectHit;

        if(Physics.Raycast(camera.transform.position, camera.transform.TransformDirection(Vector3.forward), out objectHit, projectileMaxDistance))
        {
            Debug.DrawRay(camera.transform.position, camera.transform.TransformDirection(Vector3.forward) * projectileMaxDistance, Color.red);
            if(objectHit.transform.gameObject.GetComponentInChildren<RemoteController>() != null)
            {
                RemoteController remoteController = objectHit.transform.gameObject.GetComponentInChildren<RemoteController>();
                sceneManager.raycastCall(remoteController, damage);
                
            }
        }
        else
        {
            Debug.DrawRay(camera.transform.position, camera.transform.TransformDirection(Vector3.forward) * projectileMaxDistance, Color.red);
        }
    }
}
