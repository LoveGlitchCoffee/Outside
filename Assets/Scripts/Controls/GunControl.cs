using UnityEngine;
using System.Collections;

public class GunControl : MonoBehaviour
{
    public GameObject BulletPrefab;
    public float BulletForce;
    private Vector3 forceMultiplier;

    private WaitForSeconds coolDownTime = new WaitForSeconds(0.5f);
    private bool allowedToShoot;
    
    private GameObject currentBullet;

    private Transform gun;

    private Vector3 viewportCentre;

    void Awake()
    {
        gun = transform.GetChild(0).GetChild(0);    
    }

	void Start ()
	{	    
        viewportCentre = new Vector3(0.5f,0.5f);
        forceMultiplier = new Vector3(1,1,BulletForce);

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
        var bullet = PoolManager.Instance.GetFromPool(BulletPrefab, gun.position + gun.TransformDirection(new Vector3(0, 0, 0.5f)), gun.rotation).GetComponent<BulletBehvaiour>();   
        bullet.transform.SetParent(gun);
        bullet.SetUp();
        currentBullet = bullet.gameObject;
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

        Vector3 bulletDirection = RaycastTarget();
        //bulletDirection = Vector3.Scale(bulletDirection, forceMultiplier);

        //bulletDirection = gun.transform.TransformDirection(Vector3.ClampMagnitude(bulletDirection, BulletForce));
        Debug.Log("final force " + bulletDirection);

        currentBullet.GetComponent<BulletBehvaiour>().Project(bulletDirection);
    }    

    private Vector3 RaycastTarget()
    {
        Ray ray = Camera.main.ViewportPointToRay(viewportCentre);
        RaycastHit hit;

        Physics.Raycast(ray, out hit);

        Vector3 targetPosition = hit.point;
        
        Debug.Log("target " + targetPosition);
        Debug.Log("gun: " + gun.transform.position);

        Vector3 direction = targetPosition - gun.transform.position;

        Debug.Log("direction " + direction);

        return direction;
    }

    // could do anim here
    private IEnumerator ReloadBullet()
    {
        yield return coolDownTime;

        LoadBullet();
    }
}
