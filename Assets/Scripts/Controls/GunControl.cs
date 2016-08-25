using UnityEngine;
using System.Collections;

public class GunControl : MonoBehaviour
{
    public GameObject BulletPrefab;

    [TooltipAttribute("In meters per second")]
    public float BulletVelocity;

    private WaitForSeconds coolDownTime = new WaitForSeconds(0.5f);
    private bool allowedToShoot;
    
    private GameObject currentBullet;

    private Transform gun;

    private Vector3 viewportCrosshair;

    void Awake()
    {
        gun = transform.GetChild(0).GetChild(0);    
    }

	void Start ()
	{	    
        viewportCrosshair = new Vector3(0.5f,0.56f);

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

        if (Input.GetKey(KeyCode.R))
        {
            this.PostEvent(EventID.OnReload);
        }
	}

    private void ShootBullet()
    {        
        allowedToShoot = false;
        StartCoroutine(ReloadBullet());

        this.PostEvent(EventID.OnPlayerFire);

        Vector3 bulletDirection = RaycastTarget();

        float distance = Mathf.Sqrt(Mathf.Pow(bulletDirection.x, 2) + Mathf.Pow(bulletDirection.y, 2) + Mathf.Pow(bulletDirection.z, 2));

        float multiply = BulletVelocity / distance;

        Vector3 bulletForce = bulletDirection * multiply;
        //bulletDirection = Vector3.Scale(bulletDirection, forceMultiplier);

        //bulletDirection = gun.transform.TransformDirection(Vector3.ClampMagnitude(bulletDirection, BulletForce));
        //Debug.Log("final force " + bulletForce);

        currentBullet.GetComponent<BulletBehvaiour>().Project(bulletForce);
    }    

    private Vector3 RaycastTarget()
    {
        Ray ray = Camera.main.ViewportPointToRay(viewportCrosshair);
        RaycastHit hit;

        Physics.Raycast(ray, out hit);

        Vector3 targetPosition = hit.point;
        
        //Debug.Log("target " + targetPosition);
        //Debug.Log("gun: " + gun.transform.position);

        Vector3 direction = targetPosition - gun.transform.position;

        Debug.DrawRay(gun.transform.position, direction, Color.red, 10);        

        //Debug.Log("direction " + direction);

        return direction;
    }

    // could do anim here
    private IEnumerator ReloadBullet()
    {
        yield return coolDownTime;

        LoadBullet();
    }
}
