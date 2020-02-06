using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkWeaponManager : MonoBehaviour
{

    private GameObject camera;

    [SerializeField] ClientSceneManager sceneManager = null;
    [SerializeField] float projectileMaxDistance;
    public int damage = 10;

    void Start()
    {
        camera = Camera.main.gameObject;
    }


    public void hitObjects(List<RaycastHit> hitObjects, int damagePerShot)
    {
        foreach(RaycastHit ray in hitObjects)
        {
            if(ray.transform.tag == "remotePlayer")
            {
                RemoteController remoteController = ray.transform.gameObject.GetComponentInChildren<RemoteController>();
                sceneManager.raycastCall(remoteController, damagePerShot);
            }
        }
    }

    public void hitObjects(RaycastHit hitobject, int damage)
    {
        if(hitobject.transform.tag == "remotePlayer")
        {
            RemoteController remoteController = hitobject.transform.gameObject.GetComponentInChildren<RemoteController>();
            sceneManager.raycastCall(remoteController, damage);
        }
    }

    /*private void fireRay()
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
    }*/
}
