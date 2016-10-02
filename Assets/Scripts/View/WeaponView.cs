using UnityEngine;
using System.Collections;

public class WeaponView : GameElement
{

    public GameObject SingleGun;
    public GameObject DualGun;
    public GameObject ShotGun;

    GameObject lastActive;

    bool ready;

    void Start()
    {
        lastActive = SingleGun;

        this.RegisterListener(EventID.OnSelectWeapon, (sender, param) => SwapWeapon((Weapon)param));
    }

    private void SwapWeapon(Weapon wp)
    {
        lastActive.SetActive(false);

        switch (wp)
        {
            case Weapon.SingleGun:
                {
                    lastActive = SingleGun;
                    break;
                }
            case Weapon.DualGun:
                {
                    lastActive = DualGun;
                    break;
                }
                case Weapon.Shotgun:
                {
                    lastActive = ShotGun;
                    break;
                }
        }

        //Debug.Log("active " + lastActive);
        GameManager.control.SetWeapon(lastActive.GetComponent<WeaponControl>());
        // might want to make all guns a heirarchy so can contrll
        lastActive.SetActive(true);
        lastActive.GetComponent<WeaponControl>().enabled = true;

        ready = true;
        //Debug.Log("view ready ORIGIN");
    }

    // could set somewhere else but once check can discard
    public bool ViewReady()
    {    
        return ready;
    }

    public void FalsifyView()
    {
        ready = false;
    }

    // should be in model really but hack
    public WeaponControl CurrentWeapon()
    {
        return lastActive.GetComponent<WeaponControl>();
    }

    public IEnumerator LowerWeapon()
    {
        Vector3 original = lastActive.transform.localPosition;
        Vector3 lower = original - new Vector3(0, 1, 0);
        //Debug.Log("original: " + original + ", lower " + lower);

        WaitForSeconds wait = new WaitForSeconds(0f);
        float delta = 0;

        while (lastActive.transform.localPosition.y > lower.y)
        {
            //Debug.Log("lowering to " + lastActive.transform.position.y);
            lastActive.transform.localPosition = Vector3.Lerp(original, lower, delta);
            delta += 0.05f;
            yield return wait;
        }
    }

    public IEnumerator RaiseWeapon()
    {
        Vector3 lower = lastActive.transform.localPosition;
        Vector3 original = lower + new Vector3(0,1,0); 
        
        WaitForSeconds wait = new WaitForSeconds(0f);
        float delta = 0;

        while (lastActive.transform.localPosition.y < original.y)
        {
            //Debug.Log("raising");
            lastActive.transform.localPosition = Vector3.Lerp(lower, original, delta);
            delta += 0.05f;
            yield return wait;
        }
    }
}
