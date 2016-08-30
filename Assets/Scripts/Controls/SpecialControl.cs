using UnityEngine;
using System.Collections;

public class SpecialControl : GameElement {

	public GameObject Missle;
	public ParticleSystem MissleFlash;
	public GameObject MissleLauncher;

	[Header("Properties")]
	public float MissleSpeed;
	

	void Start () {
	}
	
	void Update () {
		if (Input.GetButtonDown("Fire2") && GameManager.model.special.IsReady())
		{
			//Debug.Log("used special");
			Launch(CommonFunctions.RaycastBullet(MissleLauncher.transform, MissleSpeed));
			this.PostEvent(EventID.OnSpecialUsed);
		}		
	}	

	private void Launch(Vector3 direction)
	{
		MissleFlash.transform.position = MissleLauncher.transform.position + MissleLauncher.transform.TransformDirection(new Vector3(0, 0, 0.5f));
		MissleFlash.Play();

		var mis = Missle.GetComponent<MissleBehaviour>();
		mis.transform.position = MissleLauncher.transform.position + MissleLauncher.transform.TransformDirection(new Vector3(0, 0, 0.5f));
		mis.transform.rotation = MissleLauncher.transform.rotation;
		mis.SetUp(direction);
	}
}
