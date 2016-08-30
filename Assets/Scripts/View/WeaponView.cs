using UnityEngine;
using System.Collections;

public class WeaponView : MonoBehaviour
{

    public GameObject SingleGun;
    public GameObject DualGun;

    GameObject lastActive;


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
        }

		Debug.Log("active " + lastActive);
		// might want to make all guns a heirarchy so can contrll
        lastActive.SetActive(true);
        this.PostEvent(EventID.OnGameStart);
    }
}
