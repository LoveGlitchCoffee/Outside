using UnityEngine;
using System.Collections;

public class BarrierModel : MonoBehaviour
{

    private int health;

	public int[] thresholds;
	private int thresholdCount = 0;


    void Start()
    {
        this.RegisterListener(EventID.OnHitBarrier, (sender, param) => DecreaseHealth());
    }

    private void DecreaseHealth()
    {
		// shouldn't do but check anyway
		if (health == 0)
			return;

        health--;

		if (health == 0)
		{
			this.PostEvent(EventID.OnBarrierDown);
		}
		else if (health == thresholds[thresholdCount])
		{
			// could do here or in view
			this.PostEvent(EventID.OnBarrierLower, thresholdCount);
			thresholdCount++;
		}
    }


}
