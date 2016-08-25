using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

public class BulletModel : MonoBehaviour
{
    private int bulletsLeft;

    public int StartingBullet = 5;
    public float ReloadTime = 1;

    private WaitForSeconds reloadWait;

	void Start ()
	{
	    bulletsLeft = StartingBullet;

        reloadWait = new WaitForSeconds(ReloadTime);

        this.RegisterListener(EventID.OnPlayerFire, (sender, param) => DecreaseBullets());
	    this.RegisterListener(EventID.OnGameStart, (sender, param) => ReloadBullets());
        this.RegisterListener(EventID.OnReload , (sender, param) => ManualReload());
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
}
