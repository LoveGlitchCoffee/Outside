using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WeaponPropertyView : MonoBehaviour {

	bool ready;

	Text totalBullets;

	void Awake()
	{
		totalBullets = GetComponent<Text>();
	}

	void Start () {
		this.RegisterListener(EventID.OnChangeTotalBullets , (sender, param) => ChangeBullet((int) param));
		ready = true;
	}

	private void ChangeBullet(int total)
	{
		totalBullets.text = "/" + total.ToString();
	}	

	public bool Ready()
	{
		return ready;
	}
}
