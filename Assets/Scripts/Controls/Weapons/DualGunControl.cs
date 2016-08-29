using UnityEngine;
using System.Collections;

public class DualGunControl : GameElement
{

    [TooltipAttribute("In meters per second")]
    public float BulletVelocity;

    bool allowedToShoot;

    bool rightGunTurn;

    WaitForSeconds coolDownTime = new WaitForSeconds(0.5f);

    [Header("Guns")]
    public Transform RightGun;
    public Transform LeftGun;

    private GameObject currentBullet;

    void Start()
    {
        this.RegisterListener(EventID.OnGameStart, (sender, param) => SetLive());
    }

    void Update()
    {
        if (allowedToShoot)
        {
            if (Input.GetMouseButton(0))
            {
                ShootBullet(rightGunTurn ? RightGun : LeftGun);
            }
        }
    }

    private void SetLive()
    {
        LoadBullet(LeftGun);
        LoadBullet(RightGun);
        allowedToShoot = true;
        rightGunTurn = true;
    }

    private void ShootBullet(Transform gun)
    {
        allowedToShoot = false;

        StartCoroutine(ReloadBullet());

        this.PostEvent(rightGunTurn? EventID.OnPlayerFireRight : EventID.OnPlayerFireLeft);

        Vector3 bulletForce = CommonFunctions.RaycastBullet(gun, BulletVelocity);

		Debug.Log("bullet at: " + currentBullet.transform.position);
		Debug.Log("direction: " + bulletForce);

        currentBullet.GetComponent<BulletBehvaiour>().Project(bulletForce);

        rightGunTurn = !rightGunTurn;
    }

    private IEnumerator ReloadBullet()
    {
        if (rightGunTurn)
            LoadBullet(RightGun);
        else
            LoadBullet(LeftGun);

        yield return coolDownTime;

        allowedToShoot = true;
    }

    private void LoadBullet(Transform gun)
    {
        var bullet = PoolManager.Instance.GetFromPool(GameManager.control.TennisBall, gun.transform.position + gun.transform.TransformDirection(new Vector3(0, 0, 0.5f)), gun.rotation).GetComponent<BulletBehvaiour>();
        bullet.transform.SetParent(gun);
        bullet.SetUp();
        currentBullet = bullet.gameObject;
    }
}
