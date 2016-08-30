﻿using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

public enum Weapon
{
	SingleGun = 0,
	DualGun = 1,
}

public class WeaponModel : MonoBehaviour
{
    private int bulletsLeft;

    public int StartingBullet = 5;
    public float ReloadTime = 1;

    private WaitForSeconds reloadWait;

    Weapon wp;

	void Start ()
	{
        this.RegisterListener(EventID.OnPlayerFire, (sender, param) => DecreaseBullets());
	    this.RegisterListener(EventID.OnGameStart, (sender, param) => ReloadBullets());
        this.RegisterListener(EventID.OnReload , (sender, param) => ManualReload());        
	}

    public void SetWeapon(Weapon newWp)
    {
        wp = newWp;

        switch(wp)
        {
            case Weapon.SingleGun:
            {
                StartingBullet = 3;
                ReloadTime = 1.5f;
                reloadWait = new WaitForSeconds(ReloadTime);
                break;
            }
            case Weapon.DualGun:
            {
                StartingBullet = 6;
                ReloadTime = 1.5f;
                reloadWait = new WaitForSeconds(ReloadTime);
                break;
            }
        }
    }

    private void ManualReload()
    {
        StartCoroutine(WaitTillReloaded());
    }

    private void ReloadBullets()
    {
        bulletsLeft = StartingBullet;
        this.PostEvent(EventID.OnUpdateBullet, bulletsLeft);
    }

    private void DecreaseBullets()
    {
        bulletsLeft--;

        if (bulletsLeft == 0)
        {
            this.PostEvent(EventID.OnReload);

            StartCoroutine(WaitTillReloaded());
        }
            
        this.PostEvent(EventID.OnUpdateBullet, bulletsLeft);        
    }

    private IEnumerator WaitTillReloaded()
    {
        yield return reloadWait;

        ReloadBullets();
        this.PostEvent(EventID.OnFinishReload);        
    }

    public int BulletsLeft()
    {
        return bulletsLeft;
    }
}