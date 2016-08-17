using UnityEngine;
using System.Collections;

public class SandbagBehaviour : MonoBehaviour {

	void Start () {
		this.RegisterListener(EventID.OnHitBarrier , (sender, param) => Hit());
		this.RegisterListener(EventID.OnBarrierLower , (sender, param) => Lower());
	}
	
	private void Lower()
	{
		
	}

	private void Hit()
	{

	}
}
