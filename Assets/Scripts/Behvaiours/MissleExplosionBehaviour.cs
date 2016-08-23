using UnityEngine;
using System.Collections;

public class MissleExplosionBehaviour : MonoBehaviour {

	ParticleSystem[] ps;

	void Awake()
	{
		ps = GetComponentsInChildren<ParticleSystem>();
	}

	void Start()
	{
		this.RegisterListener(EventID.OnMissleBlow , (sender, param) => Explode((Vector3) param));
	}
	
	public void Explode(Vector3 misPos)
	{
		transform.position = misPos;
		
		for (int i = 0; i < ps.Length; i++)
		{
			ps[i].Play();
		}
	}
}
