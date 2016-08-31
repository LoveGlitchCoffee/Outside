﻿using UnityEngine;
using System.Collections;

public abstract class WeaponControl : GameElement
{

    [TooltipAttribute("In meters per second")]
    public float BulletVelocity;

	public float CoolDownTime;

    protected WaitForSeconds coolDownTime;

    protected bool allowedToShoot = true;

    protected GameObject currentBullet;

    protected bool playerLose;

    protected virtual void Start()
    {
		coolDownTime = new WaitForSeconds(CoolDownTime);

        this.RegisterListener(EventID.OnPlayerDie, (sender, param) => DeActivate());
        this.RegisterListener(EventID.OnGameStart, (sender, param) => SetLive());
        this.RegisterListener(EventID.OnPlayerDie, (sender, param) => SetDead());
        this.RegisterListener(EventID.OnReload, (sender, param) => DeActivate());
        this.RegisterListener(EventID.OnFinishReload, (sender, param) => Activate());
    }

    protected void Activate()
    {
        if (!playerLose)
            enabled = true;
    }

    protected void DeActivate()
    {
        enabled = false;
    }

    protected virtual void SetLive()
    {
        playerLose = false;
    }

    protected virtual void SetDead()
    {
        playerLose = true;
    }

    protected void LoadBullet(Transform gun)
    {
        var bullet = PoolManager.Instance.GetFromPool(GameManager.control.TennisBall, gun.transform.position + gun.transform.TransformDirection(new Vector3(0, 0, 0.5f)), gun.rotation).GetComponent<BulletBehvaiour>();
        bullet.transform.SetParent(gun);
        bullet.SetUp();
        currentBullet = bullet.gameObject;
    }

    protected virtual void ShootBullet(Transform gun)
    {
        allowedToShoot = false;

        StartCoroutine(ReloadBullet());

        Vector3 bulletForce = CommonFunctions.RaycastBullet(gun, BulletVelocity);

        /*Debug.Log("bullet at: " + currentBullet.transform.position);
		Debug.Log("direction: " + bulletForce);*/

        if (currentBullet != null)
            currentBullet.GetComponent<BulletBehvaiour>().Project(bulletForce);
    }


    protected abstract IEnumerator ReloadBullet();
}
