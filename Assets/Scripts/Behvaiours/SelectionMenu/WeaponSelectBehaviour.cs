using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WeaponSelectBehaviour : GameElement {

	Button selectable;

	public Weapon SelectedWeapon; 

	void Awake()
	{
		selectable = GetComponent<Button>();
	}

	void Start () 
	{
		selectable.onClick.AddListener(SelectWeapon);
	}
	
	private void SelectWeapon()
	{
		this.PostEvent(EventID.OnPressWeapon, SelectedWeapon);
	}
}
