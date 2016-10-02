using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BloodView : MonoBehaviour
{

    Image blood;

    void Awake()
    {
        blood = GetComponent<Image>();
    }

    void Start()
    {
        this.RegisterListener(EventID.OnPlayerDie, (sender, param) => BloodHUDEffect());
        this.RegisterListener(EventID.OnGameEnd, (sender, param) => HideBlood());
    }

    private void BloodHUDEffect()
    {
        StartCoroutine(BloodHUD());
    }

    IEnumerator BloodHUD()
    {
        yield return new WaitForSeconds(0.5f);

		blood.enabled = true;
		blood.color = Color.white;

		yield return new WaitForSeconds(0.1f);
		
		blood.color = new Color(0.6f,0.6f,0.6f, 1);
    }

    private void HideBlood()
    {
        blood.enabled = false;
    }
}
