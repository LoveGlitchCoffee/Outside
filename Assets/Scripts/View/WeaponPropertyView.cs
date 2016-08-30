using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WeaponPropertyView : MonoBehaviour {

	Text totalBullets;

	void Start () {
		this.RegisterListener(EventID.OnChangeTotalBullets , (sender, param) => ChangeBullet((int) param));
	}

	private void ChangeBullet(int total)
	{
		totalBullets.text = "/" + total.ToString();
	}	
}
