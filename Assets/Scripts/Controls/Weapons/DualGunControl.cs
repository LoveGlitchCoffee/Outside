using UnityEngine;
using System.Collections;

public class DualGunControl : WeaponControl
{

    bool rightGunTurn;

    [Header("Guns")]
    public Transform RightGun;
    public Transform LeftGun;

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

    protected override void SetLive()
    {
        base.SetLive();

        LoadBullet(LeftGun);
        LoadBullet(RightGun);

        rightGunTurn = true;
    }

    protected override void ShootBullet(Transform gun)
    {
        base.ShootBullet(gun);

        this.PostEvent(rightGunTurn ? EventID.OnPlayerFireRight : EventID.OnPlayerFireLeft);

        rightGunTurn = !rightGunTurn;
    }

    protected override IEnumerator ReloadBullet()
    {
        if (rightGunTurn)
            LoadBullet(RightGun);
        else
            LoadBullet(LeftGun);

        yield return coolDownTime;

        allowedToShoot = true;
    }


}
