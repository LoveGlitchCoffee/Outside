using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBarView : MonoBehaviour {

	Image bar;
	Text desc;

	void Awake()
	{
		int childNo = transform.childCount;

		bar = transform.GetChild(childNo - 2).GetComponent<Image>();
		desc = transform.GetChild(childNo - 1).GetComponent<Text>();
	}

	void Start () 
	{
		this.RegisterListener(EventID.OnGameStart , (sender, param) => ResetHealth());
		this.RegisterListener(EventID.OnUpdateHealth , (sender, param) => UpdateHealth((int) param));	
	}

	private void ResetHealth()
	{
		bar.fillAmount = 1;
	}

	private void UpdateHealth(int amount)
	{
		Debug.Log("new amount " + amount);
		StartCoroutine(TweenFill((float)amount));
	}
	
	// similar to special, could refactor but might not be worth
	private IEnumerator TweenFill(float newAmount)
    {
        float oldAmount = bar.fillAmount;

        if (oldAmount != 0)
        {
            WaitForSeconds wait = new WaitForSeconds(0);
            float delta = 0;
			float newBarAmount = newAmount/100;

            while (bar.fillAmount > newBarAmount)
            {
                bar.fillAmount = Mathf.Lerp(oldAmount, newBarAmount, delta);
                desc.text = ((int)(bar.fillAmount * 100)).ToString();
                delta += 0.1f;
                yield return wait;
            }
        }
    }
}
