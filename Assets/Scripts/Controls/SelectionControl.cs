using UnityEngine;
using System.Collections;

public class SelectionControl : GameElement {

	public void GoInGame()
	{
		this.PostEvent(EventID.OnGameStart);
	}

	public void SelectSingle()
	{
		GameManager.model.weapon.SetWeapon(Weapon.SingleGun);
	}

	public void SelectDual()
	{
		GameManager.model.weapon.SetWeapon(Weapon.DualGun);
	}
}
