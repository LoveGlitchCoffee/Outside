using UnityEngine;
using System.Collections;

public class ShotgunControl : WeaponControl
{

    GameObject[] bullets;

    Transform[] muzzles;

    const int muzzleCount = 4;

    void Awake()
    {
        bullets = new GameObject[muzzleCount];
        muzzles = new Transform[muzzleCount];

        for (int i = 0; i < muzzleCount; i++)
        {
            muzzles[i] = transform.GetChild(i);
        }
    }

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
                ShootBullet();
            }

            if (Input.GetKey(KeyCode.R))
            {
                if (!(GameManager.model.weapon.BulletsLeft() == GameManager.model.weapon.StartingBullet))
                    this.PostEvent(EventID.OnReload);
            }
        }
    }

    private void LoadBullet()
    {
        for (int i = 0; i < muzzleCount; i++)
        {
            Transform gun = muzzles[i];

            var bullet = PoolManager.Instance.GetFromPool(GameManager.control.TennisBall, gun.transform.position + gun.transform.TransformDirection(new Vector3(0, 0, 0.5f)), gun.rotation).GetComponent<BulletBehvaiour>();

            bullet.transform.SetParent(gun);
            bullet.SetUp();

            bullets[i] = bullet.gameObject;
        }
    }

    protected override void SetLive()
    {
        LoadBullet();

        base.SetLive();
    }

    private void ShootBullet()
    {
        allowedToShoot = false;

        StartCoroutine(ReloadBullet());

        for (int i = 0; i < muzzleCount; i++)
        {
            Vector3 bulletForce = CommonFunctions.RaycastBullet(muzzles[i], BulletVelocity);

            switch (i)
            {
                case 0:
                    {
                        bulletForce += new Vector3(Random.value, Random.value, 0);
                        break;
                    }
                    case 1:
                    {
                        bulletForce += new Vector3(Random.value, Random.value, 0);
                        break;
                    }
                    case 2:
                    {
                        bulletForce -= new Vector3(Random.value, Random.value, 0);
                        break;
                    }
                    case 3:
                    {
                        bulletForce -= new Vector3(Random.value, Random.value, 0);
                        break;
                    }
            }

            bullets[i].GetComponent<BulletBehvaiour>().Project(bulletForce);
        }
    }

    protected override IEnumerator ReloadBullet()
    {
        yield return coolDownTime;

        LoadBullet();

        allowedToShoot = true;
    }
}
