using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteController : MonoBehaviour
{
    public int id;
    private Renderer objectRender;

    [SerializeField] Material[] mats;

    public void initRemote(int newID)
    {
        this.id = newID;

        setColor(newID);
       
    }

    private void setColor(int id)
    {
        objectRender = gameObject.GetComponent<Renderer>();
        switch (id)
        {
            case 0:
                objectRender.material = mats[0];
                break;
            case 1:
                objectRender.material = mats[1];
                break;
            case 2:
                objectRender.material = mats[2];
                break;
            case 3:
                objectRender.material = mats[3];
                break;
        }
    }

}
