using UnityEngine;
using System.Collections;

public class MissleExplosionBehaviour : MonoBehaviour {

	ParticleSystem[] ps;
	AudioSource audio;

	void Awake()
	{
		ps = GetComponentsInChildren<ParticleSystem>();
		audio = GetComponent<AudioSource>();
	}

	void Start()
	{
		this.RegisterListener(EventID.OnMissleBlow , (sender, param) => Explode((Vector3) param));
	}
	
	public void Explode(Vector3 misPos)
	{
		transform.position = misPos;
		audio.Play();
		
		for (int i = 0; i < ps.Length; i++)
		{
			ps[i].Play();
		}
	}
}
