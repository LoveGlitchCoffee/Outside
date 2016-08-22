using UnityEngine;
using System.Collections;

public class MissleExplosionBehaviour : MonoBehaviour {

	ParticleSystem ps;

	void Awake()
	{
		ps = GetComponent<ParticleSystem>();
	}

	void Start()
	{
		this.RegisterListener(EventID.OnMissleBlow , (sender, param) => Explode((Vector3) param));
	}
	
	public void Explode(Vector3 misPos)
	{
		transform.position = misPos;
		ps.Play();
	}
}
