using UnityEngine;
using System.Collections;

public class GameView : MonoBehaviour {

	private Coroutine PunchEffect;

	public WeaponPropertyView WeaponProperty;	
	public WeaponView Weapon;

	void Start()
	{
		this.RegisterListener(EventID.OnEnemyHit , (sender, param) => ActivatePunchEffect());
	}	

	void ActivatePunchEffect()
	{
		if (PunchEffect == null)
			PunchEffect = StartCoroutine(PunchEffectCoroutine());
	}

	IEnumerator PunchEffectCoroutine()
	{
		WaitForSeconds wait = new WaitForSeconds(0.006f);
		Time.timeScale = 0.1f;

		yield return wait;

		Time.timeScale = 1;

		PunchEffect = null;
	}
	
}
