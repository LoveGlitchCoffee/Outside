using UnityEngine;
using System.Collections;

public class GunControl : MonoBehaviour
{
    public GameObject BulletPrefab;
    public float BulletForce;

    private WaitForSeconds coolDownTime = new WaitForSeconds(0.5f);
    private bool allowedToShoot;
    
    private GameObject currentBullet;

    private Transform gun;

    void Awake()
    {
        gun = transform.GetChild(1);    
    }

	void Start ()
	{	    
	    LoadBullet();
        
        this.RegisterListener(EventID.OnGameStart, (sender, param) => Activate());
        this.RegisterListener(EventID.OnPlayerDie, (sender, param) => DeActivate());
        this.RegisterListener(EventID.OnReload, (sender, param) => DeActivate());
	    this.RegisterListener(EventID.OnFinishReload, (sender, param) => Activate());

        DeActivate();
	}

    public void DeActivate()
    {
        enabled = false;
    }

    public void Activate()
    {
        enabled = true;
    }

    private void LoadBullet()
    {
        var bullet = PoolManager.Instance.GetFromPool(BulletPrefab);
        bullet.transform.position = gun.position + gun.TransformDirection(new Vector3(0,0,0.5f));
        bullet.transform.rotation = gun.rotation;
        bullet.transform.SetParent(gun);

        currentBullet = bullet;
        allowedToShoot = true;
    }

    void Update ()
	{
	    if (allowedToShoot && Input.GetMouseButton(0))
	        ShootBullet();
	}

    private void ShootBullet()
    {        
        allowedToShoot = false;
        StartCoroutine(ReloadBullet());

        this.PostEvent(EventID.OnPlayerFire);
        currentBullet.GetComponent<BulletBehvaiour>().Project(transform.GetChild(0).TransformDirection(new Vector3(0,0,BulletForce)));
    }    

    // could do anim here
    private IEnumerator ReloadBullet()
    {
        yield return coolDownTime;

        LoadBullet();
    }
}
