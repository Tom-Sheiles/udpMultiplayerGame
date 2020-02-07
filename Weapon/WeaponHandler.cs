using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] Weapon[] weapons = new Weapon[2];
    GameObject[] weaponViewModels = new GameObject[2];
    Animator[] viewModelAnimators = new Animator[2];
    [SerializeField] Transform raycastOrigin;
    [SerializeField] Transform viewModelPosition;
    [SerializeField] Transform bulletOrigin;

    [SerializeField] GameEvent switchWeaponEvent;
    [SerializeField] GameEvent weaponSwitched;
    [SerializeField] GameObject bulletVFX;

    NetworkWeaponManager networkWeaponManager;
    Transform playerCamera;

    private int selectedWeapon = 0;
    public int numberOfWeapons = 1;

    private void Start()
    {

        networkWeaponManager = gameObject.GetComponent<NetworkWeaponManager>();

        foreach(Weapon weapon in weapons)
        {
            if(weapon != null)
            weaponViewModels[0] = weapon.initialize(viewModelPosition);
            viewModelAnimators[0] = weaponViewModels[0].GetComponent<Animator>();
            weapons[0].setAnimator(viewModelAnimators[0]); 
        }
    }

    private void Awake()
    {
        playerCamera = Camera.main.gameObject.transform;
    }

    private void Update()
    {

        if (!weapons[selectedWeapon].isAutomatic)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                handleWeapon();
            }
        }
        else
        {
            if (Input.GetButton("Fire1"))
            {
                handleWeapon();
                
            }
        }
        

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(weapons[selectedWeapon].reloadCoroutine());
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && selectedWeapon != 0 && !weapons[selectedWeapon].isReloading)
        {
            weapons[selectedWeapon].animator.ResetTrigger("Reload");
            switchTo(0);
            
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && weapons[1] != null && selectedWeapon != 1 && !weapons[selectedWeapon].isReloading)
        {
            weapons[selectedWeapon].animator.ResetTrigger("Reload");
            switchTo(1);
            
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            dropSecondary();
        }

        if (weapons[selectedWeapon].isReloading)
        {
            viewModelAnimators[selectedWeapon].SetTrigger("Reload");
        }
        else
        {
            viewModelAnimators[selectedWeapon].ResetTrigger("Reload");
        }
    }

    private void dropSecondary()
    {
        if (weapons[1] == null)
            return;

        switchTo(0);

        weapons[1] = null;
        numberOfWeapons--;
    }

    public void Pickup(Weapon weapon)
    {
        Debug.Log("picked up a " + weapon);
        switchWeaponEvent.Raise();
        weapons[1] = weapon;
        weaponViewModels[1] = weapons[1].initialize(viewModelPosition);
        viewModelAnimators[1] = weaponViewModels[1].GetComponent<Animator>();
        weapons[1].setAnimator(viewModelAnimators[1]);
        selectedWeapon = 1;
        numberOfWeapons++;
        weaponSwitched.Raise();
    }

    private void switchTo(int weaponSlot)
    {
        switchWeaponEvent.Raise();
        selectedWeapon = weaponSlot;
        weaponViewModels[selectedWeapon].SetActive(true);
        weaponSwitched.Raise();
    }

    private void handleWeapon()
    {
        if (weapons[selectedWeapon].GetType() ==  typeof(RaycastWeapon))
        {
            RaycastHit hitObject = ((RaycastWeapon)weapons[selectedWeapon]).instanceRay(raycastOrigin, bulletOrigin);
        }

        if(weapons[selectedWeapon].GetType() == typeof(MultiRaycast))
        {
            if (((MultiRaycast)weapons[selectedWeapon]).isReloading)
            {
                ((MultiRaycast)weapons[selectedWeapon]).hasShotWhenReload = true;
            }
            List<RaycastHit> hitObjects = ((MultiRaycast)weapons[selectedWeapon]).multiRaycast(raycastOrigin, bulletOrigin);
            networkWeaponManager.hitObjects(hitObjects, ((MultiRaycast)weapons[selectedWeapon]).damagePerShot);
            
        }
    }

    public Weapon getSelectedWeapon()
    {
        return weapons[selectedWeapon];
    }
}
