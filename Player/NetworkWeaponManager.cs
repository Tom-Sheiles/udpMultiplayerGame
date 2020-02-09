using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkWeaponManager : MonoBehaviour
{

    private GameObject camera;

    [SerializeField] ClientSceneManager sceneManager = null;
    [SerializeField] float projectileMaxDistance;
    [SerializeField] GameObject damageNumberPrefab;
    public float damageNumberOffset = 0.45f;

    void Start()
    {
        camera = Camera.main.gameObject;
    }


    public void hitObjects(List<RaycastHit> hitObjects, int damagePerShot)
    {
        var totalDamage = 0;
        var raypos = new RaycastHit();

        foreach(RaycastHit ray in hitObjects)
        {
            if(ray.transform.tag == "remotePlayer")
            {
                RemoteController remoteController = ray.transform.gameObject.GetComponentInChildren<RemoteController>();

                totalDamage += damagePerShot;

                sceneManager.raycastCall(remoteController, damagePerShot);

                raypos = ray;
            }
        }

        try
        {
            var damageNumberPos = raypos.transform.position;
            damageNumberPos.x = Random.Range(raypos.transform.position.x - damageNumberOffset, raypos.transform.position.x + damageNumberOffset);
            damageNumberPos.y = Random.Range(raypos.transform.position.y, raypos.transform.position.y + damageNumberOffset);
            damageNumberPos.z = Random.Range(raypos.transform.position.z - damageNumberOffset, raypos.transform.position.z + damageNumberOffset);

            var damageNumber = Instantiate(damageNumberPrefab, damageNumberPos, raypos.transform.rotation);
            damageNumber.GetComponentInChildren<Text>().text = "-" + totalDamage.ToString();
        }
        catch
        {

        }
    }

    public void hitObjects(RaycastHit hitobject, int damage)
    {

        try
        {
            if (hitobject.transform.tag == "remotePlayer")
            {
                RemoteController remoteController = hitobject.transform.gameObject.GetComponentInChildren<RemoteController>();
                sceneManager.raycastCall(remoteController, damage);
            }
        }
        catch
        {

        }
       
    }
}
