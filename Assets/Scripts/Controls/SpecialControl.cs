using UnityEngine;
using System.Collections;

public class SpecialControl : GameElement {

	public GameObject Missle;
	public GameObject Gun;
	

	void Start () {
	
	}
	
	void Update () {
		if (Input.GetButtonDown("Fire2") && GameManager.model.special.IsReady())
		{
			//Debug.Log("used special");
			Launch((transform.GetChild(0).TransformDirection(new Vector3(0,0,1))));
			this.PostEvent(EventID.OnSpecialUsed);
		}		
	}	

	private void Launch(Vector3 direction)
	{
		var mis = Missle.GetComponent<MissleBehaviour>();
		mis.transform.position = Gun.transform.position + Gun.transform.TransformDirection(new Vector3(0, 0, 0.5f));
		mis.transform.rotation = Gun.transform.rotation;
		mis.SetUp(direction);
	}
}
