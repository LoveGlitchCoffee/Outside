using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BulletView : MonoBehaviour
{
    private Text bulletText;

    void Awake()
    {
        bulletText = GetComponent<Text>();
    }

	void Start ()
    {
        this.RegisterListener(EventID.OnUpdateBullet, (sender, param) => UpdateBullet((int) param));    	
	}

    private void UpdateBullet(int bulletLeft)
    {
        bulletText.text = bulletLeft.ToString();
    }	
}
