using UnityEngine;
using System.Collections;

public class ImpactParticleBehaviour : MonoBehaviour {

	ParticleSystem particles;

	void Awake () 
	{
		particles = GetComponent<ParticleSystem>();
	}
	
	public void Play(Quaternion rot)
	{
		Debug.Log("Play impact");

		particles.startRotation3D = new Vector3(0,0,rot.eulerAngles.z * Mathf.Deg2Rad);
		particles.Play();

		StartCoroutine(WaitTillEnd());
	}

	IEnumerator WaitTillEnd()
	{		
		WaitForSeconds wait = new WaitForSeconds(0);

		while (particles.isPlaying)
		{
			yield return wait;
		}

		PoolManager.Instance.ReturnToPool(gameObject);
	}
}
