using UnityEngine;
using System.Collections;

public class SpecialControl : GameElement {

	public GameObject Missle;
	public ParticleSystem MissleFlash;
	public GameObject Gun;

	[Header("Properties")]
	public float MissleSpeed;
	

	void Start () {
	}
	
	void Update () {
		if (Input.GetButtonDown("Fire2"))
		{
			//Debug.Log("used special");
			Launch(CommonFunctions.RaycastBullet(Gun.transform, MissleSpeed));
			this.PostEvent(EventID.OnSpecialUsed);
		}		
	}	

	private void Launch(Vector3 direction)
	{
		MissleFlash.transform.position = Gun.transform.position + Gun.transform.TransformDirection(new Vector3(0, 0, 0.5f));
		MissleFlash.Play();

		var mis = Missle.GetComponent<MissleBehaviour>();
		mis.transform.position = Gun.transform.position + Gun.transform.TransformDirection(new Vector3(0, 0, 0.5f));
		mis.transform.rotation = Gun.transform.rotation;
		mis.SetUp(direction);
	}
}
