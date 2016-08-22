using UnityEngine;
using System.Collections;

public class SpecialControl : GameElement {

	void Start () {
	
	}
	
	void Update () {
		if (Input.GetButtonDown("Fire2") && GameManager.model.special.IsReady())
		{
			this.PostEvent(EventID.OnSpecialUsed);
		}
	}	
}
