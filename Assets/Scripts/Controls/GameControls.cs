using UnityEngine;
using System.Collections;

public class GameControls : MonoBehaviour {

	public GameObject TennisBall;

	public MovementController move;
	
	[Header("Weapons")]
	public GunControl gun;
	public DualGunControl dual;

	void Start () {
		this.RegisterListener(EventID.OnGameStart , (sender, param) => Activate());
		this.RegisterListener(EventID.OnGameEnd , (sender, param) => DeActivate());
	}
	
	private void Activate()
	{
		//gun.LoadBullet();
		//gun.enabled = true;
		move.enabled = true;
	}

	private void DeActivate()
	{
		gun.enabled = false;
		move.enabled = false;
	}
}
