using UnityEngine;
using System.Collections;

public class SpecialControl : GameElement {
	
	[Header("Properties")]
	public float MissleSpeed;
	

	void Start () {
	}
	
	void Update () {
		if (Input.GetButtonDown("Fire2") && GameManager.model.special.IsReady() && GameManager.control.CanShoot())
		{
			this.PostEvent(EventID.OnSpecialUsed, MissleSpeed);
			StartCoroutine(WaitTillFinishSpecial());
		}		
	}	

	private IEnumerator WaitTillFinishSpecial()
	{
		yield return new WaitForSeconds(2);

		this.PostEvent(EventID.OnFinishSpecial);
	}
	
}
