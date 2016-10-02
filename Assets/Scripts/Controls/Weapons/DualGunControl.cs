using UnityEngine;
using System.Collections;

public class DualGunControl : WeaponControl
{

    bool rightGunTurn;

    [Header("Guns")]
    public Transform RightGun;
    public Transform LeftGun;

    GameObject altBullet;

    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        if (allowedToShoot)
        {
            if (Input.GetMouseButton(0))
            {
                ShootBullet(rightGunTurn ? RightGun : LeftGun);
            }

            if (Input.GetKey(KeyCode.R))
            {
                if (!(GameManager.model.weapon.BulletsLeft() == GameManager.model.weapon.StartingBullet))
                    this.PostEvent(EventID.OnReload);
            }
        }
    }

    protected override void LoadBullet(Transform gun)
    {
        if (!gameObject.activeSelf)
            return;

        var bullet = PoolManager.Instance.spawnObject(GameManager.control.TennisBall, gun.transform.position + gun.transform.TransformDirection(new Vector3(0, 0, 0.5f)), gun.rotation).GetComponent<BulletBehvaiour>();
        //Debug.Log("gun at " + gun.position);
        bullet.transform.SetParent(gun);
        Debug.Log("got bullet " + bullet.gameObject.name);
        bullet.SetUp();

        if (rightGunTurn)
            currentBullet = bullet.gameObject;
        else
            altBullet = bullet.gameObject;
    }

    protected override void SetLive()
    {
        LoadBullet(LeftGun);

        rightGunTurn = true;

        LoadBullet(RightGun);

        base.SetLive();
        Debug.Log("children live");        
    }

    protected override void ShootBullet(Transform gun)
    {
        allowedToShoot = false;

        StartCoroutine(ReloadBullet());

        Vector3 bulletForce = CommonFunctions.RaycastBullet(gun, BulletVelocity);

        if (rightGunTurn)
        {
            if (currentBullet != null)
            {
                currentBullet.GetComponent<BulletBehvaiour>().Project(bulletForce);
                Debug.Log("shot "+ currentBullet.name);                
            }
        }
        else
        {
            if (altBullet != null)
            {
                altBullet.GetComponent<BulletBehvaiour>().Project(bulletForce);                
                Debug.Log("shot "+ altBullet.name);
            }
        }

        this.PostEvent(rightGunTurn ? EventID.OnPlayerFireRight : EventID.OnPlayerFireLeft);
    }

    protected override IEnumerator ReloadBullet()
    {
        yield return coolDownTime;

        if (rightGunTurn)
            LoadBullet(RightGun);
        else
        {
            LoadBullet(LeftGun);
        }

        rightGunTurn = !rightGunTurn;

        allowedToShoot = true;
    }


}
