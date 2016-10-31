using UnityEngine;
using System.Collections;

public class GameView : MonoBehaviour {

	public WeaponPropertyView WeaponProperty;	
	public WeaponView Weapon;

	public GameObject PauseMenu;

	bool pausing = false;

	void Start()
	{
		// could be in own behaviour script
		this.RegisterListener(EventID.TogglePause , (sender, param) => TogglePauseMenu());
		this.RegisterListener(EventID.OnGameEnd , (sender, param) => UnPause());
	}		

	private void UnPause()
	{
		pausing = false;

		PauseMenu.SetActive(false);
	}

	private void TogglePauseMenu()
	{
		if (!pausing)
		{
			Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true; 			
		}
		else
		{			
			Cursor.lockState = CursorLockMode.Locked;
    	    Cursor.visible = false;           
		}

		PauseMenu.SetActive(!pausing);

		pausing = !pausing;
	}
}
