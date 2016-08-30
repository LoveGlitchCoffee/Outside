using UnityEngine;
using System.Collections;

public class SelectionControl : GameElement {
	
	public void GoInGame()
	{
		Debug.Log("Selected weapon: " + GameManager.selectionModel.SelectedWeapon());
		this.PostEvent(EventID.OnSelectWeapon, GameManager.selectionModel.SelectedWeapon());
	}

	
}
