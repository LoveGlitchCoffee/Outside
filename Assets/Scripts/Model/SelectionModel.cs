using UnityEngine;
using System.Collections;

public class SelectionModel : MonoBehaviour
{

    Weapon wp;

	void Start()
	{
		wp = Weapon.SingleGun;
		this.RegisterListener(EventID.OnPressWeapon , (sender, param) => SelectWeapon((Weapon) param));
	}

    public void SelectWeapon(Weapon newWp)
    {
        wp = newWp;
    }

	public Weapon SelectedWeapon()
	{
		return wp;
	}
}
