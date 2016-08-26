using UnityEngine;
using System.Collections;

public class GameControls : MonoBehaviour {

	public GunControl gun;
	public MovementController move;

	void Start () {
		this.RegisterListener(EventID.OnGameStart , (sender, param) => Activate());
		this.RegisterListener(EventID.OnGameEnd , (sender, param) => DeActivate());
	}
	
	private void Activate()
	{
		gun.enabled = true;
		move.enabled = true;
	}

	private void DeActivate()
	{
		gun.enabled = false;
		move.enabled = false;
	}
}
