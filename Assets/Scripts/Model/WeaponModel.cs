using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

public enum Weapon
{
    SingleGun = 0,
    DualGun = 1,
}

public class WeaponModel : GameElement
{
    private int bulletsLeft;

    public int StartingBullet = 5;
    public float ReloadTime = 1;

    private WaitForSeconds reloadWait;

    Weapon wp;

    void Start()
    {
        this.RegisterListener(EventID.OnSelectWeapon, (sender, param) => SetWeapon((Weapon)param));
        this.RegisterListener(EventID.OnPlayerFire, (sender, param) => DecreaseBullets());
        this.RegisterListener(EventID.OnPlayerFireRight, (sender, param) => DecreaseBullets());
        this.RegisterListener(EventID.OnPlayerFireLeft, (sender, param) => DecreaseBullets());
        this.RegisterListener(EventID.OnReload, (sender, param) => ManualReload());
    }

    private void SetWeapon(Weapon newWp)
    {
        wp = newWp;

        switch (wp)
        {
            case Weapon.SingleGun:
                {
                    StartingBullet = 3;
                    ReloadTime = 1.5f;
                    reloadWait = new WaitForSeconds(ReloadTime);
                    break;
                }
            case Weapon.DualGun:
                {
                    StartingBullet = 6;
                    ReloadTime = 1.5f;
                    reloadWait = new WaitForSeconds(ReloadTime);
                    break;
                }
        }

        StartCoroutine(WaitTillReady(StartingBullet));
    }

    private IEnumerator WaitTillReady(int StartingBullet)
    {
        WaitForSeconds wait = new WaitForSeconds(0);

        while (!GameManager.view.Weapon.CurrentWeapon().IsReady())
        {
            yield return wait;
        }
        // may want to check GameControl Weapon is assigned

        this.PostEvent(EventID.OnGameStart);

        while (!GameManager.view.WeaponProperty.Ready())
        {
            yield return wait;
        }

        this.PostEvent(EventID.OnChangeTotalBullets, StartingBullet);
        ReloadBullets();
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

    public int BulletsLeft()
    {
        return bulletsLeft;
    }
}
