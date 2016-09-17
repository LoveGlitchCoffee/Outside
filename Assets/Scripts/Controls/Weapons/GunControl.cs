using UnityEngine;
using System.Collections;

public class GunControl : WeaponControl
{   

    private Transform gun;

    void Awake()
    {
        gun = transform.GetChild(0);
    }

    protected override void Start()
    {
        Debug.Log("called");
        base.Start();
    }    

    void Update()
    {
        if (allowedToShoot)
        {
            //Debug.Log("allowed to shoot");
            if (Input.GetMouseButton(0))
                ShootBullet(gun);

            if (Input.GetKey(KeyCode.R))
            {
                if (!(GameManager.model.weapon.BulletsLeft() == GameManager.model.weapon.StartingBullet))
                    this.PostEvent(EventID.OnReload);
            }
        }
    }

    protected override void SetLive()
    {
        Debug.Log("override live");
        base.SetLive();
        LoadBullet(gun);
        Debug.Log("loaded bullet");
    }

    protected override void ShootBullet(Transform gun)
    {       
        base.ShootBullet(gun);

        this.PostEvent(EventID.OnPlayerFire);       
    }

    // could do anim here
    protected override IEnumerator ReloadBullet()
    {
        yield return coolDownTime;

        LoadBullet(gun);

        allowedToShoot = true;        
    }

    
}
