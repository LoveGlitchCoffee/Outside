using UnityEngine;
using System.Collections;

public class GameControls : GameElement {

	// prefab. make sense here? maybe model
	public GameObject TennisBall;

	public MovementController Movement;
	
	[Header("Weapon")]
	WeaponControl weapon;

	void Start () {
		this.RegisterListener(EventID.OnGameStart , (sender, param) => Activate());
		this.RegisterListener(EventID.OnGameEnd , (sender, param) => DeActivate());
	}

	void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.model.Set()) // use set to make sure in game. could use isPlaying
		{
            this.PostEvent(EventID.TogglePause);
		}
    }

	public void SetWeapon(WeaponControl wp)
	{
		weapon = wp;
	}

	public bool CanShoot()
	{
		return weapon.AllowedToShoot();
	}

	private void Activate()
	{
		weapon.enabled = true;
		Movement.enabled = true;
	}

	private void DeActivate()
	{
		weapon.enabled = false;
		Movement.enabled = false;
	}
}
