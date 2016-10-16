using UnityEngine;
using System.Collections;

public class CameraShaker : MonoBehaviour {

	public float ShakeAmount = 0.25f; // linear 'scale' of shake
	public float DecreaseFactor = 1.0f; // by how much does shake decrease each time

	private float shake; // amount of shakes

	void Start () {
		this.RegisterListener(EventID.OnScreenShake , (sender, param) => ScreenShake((float)param));
	
	}

	
	private void ScreenShake(float shakeCount)
    {
		shake = shakeCount;

        StartCoroutine(ShakeCamera());
    }

    IEnumerator ShakeCamera()
    {
        Vector3 originalPos = transform.localPosition;
        float y = originalPos.y;

        WaitForSeconds wait = new WaitForSeconds(0f);

		while (shake > 0)
		{
	        transform.localPosition = Random.insideUnitSphere * ShakeAmount * shake;
			transform.localPosition += new Vector3(0,y,0);

			shake -= Time.deltaTime * DecreaseFactor;

			yield return wait;			
		}

        transform.localPosition = originalPos;
    }	
}
