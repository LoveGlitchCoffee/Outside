using UnityEngine;
using System.Collections;

public class SpecialControl : GameElement {

	public GameObject Missle;
	public ParticleSystem MissleFlash;
	public GameObject Gun;

	[Header("Properties")]
	public float MissleSpeed;
	
	private Vector3 viewportCrosshair;

	void Start () {
	viewportCrosshair = new Vector3(0.5f, 0.56f);
	}
	
	void Update () {
		if (Input.GetButtonDown("Fire2"))
		{
			//Debug.Log("used special");
			Launch(CalculateForce());
			this.PostEvent(EventID.OnSpecialUsed);
		}		
	}	

	// could be common somewhere
	private Vector3 CalculateForce()
	{
		Ray ray = Camera.main.ViewportPointToRay(viewportCrosshair);
		RaycastHit hit;

		Physics.Raycast(ray, out hit);

		Vector3 contact = hit.point;

		Vector3 direction = contact - Gun.transform.position;

		float distance = Mathf.Sqrt(Mathf.Pow(direction.x, 2) + Mathf.Pow(direction.y, 2) + Mathf.Pow(direction.z, 2));

		float multiply = MissleSpeed / distance;

		Vector3 missleForce = direction * multiply;

		return missleForce;
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
