using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CrosshairBehaviour : MonoBehaviour {

	Image sprite;

	Color bright = new Color(1,1,1,0.6f);
	Color dim;

	void Awake()
	{
		sprite = transform.GetChild(0).GetComponent<Image>();
	}

	void Start () {
		dim = sprite.color;

		this.RegisterListener(EventID.OnEnemyDie , (sender, param) => Shine());
	}

	private void Shine()
	{
		StartCoroutine(QuickShine());
	}

	private IEnumerator QuickShine()
	{
		WaitForSeconds wait = new WaitForSeconds(0);
		float delta = 0;

		while (delta <= 1)
		{
			sprite.color = Color.Lerp(dim, bright, delta);
			delta += 0.2f;
			yield return wait;
		}

		delta = 0;

		while (delta <= 1)
		{
			sprite.color = Color.Lerp(bright, dim, delta);
			delta += 0.2f;
			yield return wait;
		}
	}
	
	
}
