﻿using UnityEngine;
using System.Collections;

public class BarrierModel : MonoBehaviour
{

    private int health = 100;

	public int[] thresholds;
	private int thresholdCount = 0;


    void Start()
    {
        this.RegisterListener(EventID.OnHitBarrier, (sender, param) => DecreaseHealth());
		this.RegisterListener(EventID.OnGameStart , (sender, param) => ResetModel());
    }
	
	// dun forget
	private void ResetModel()
	{
		health = 100;
		thresholdCount = 0;
	}

    private void DecreaseHealth()
    {
		// shouldn't do but check anyway
		if (health == 0)
			return;

        health--;
		this.PostEvent(EventID.OnUpdateHealth, health);
		//Debug.Log("health " + health);
		
		if (health == 0)
		{
			this.PostEvent(EventID.OnBarrierDown);
		}
		else if (health == thresholds[thresholdCount])
		{
			// could do here or in view
			this.PostEvent(EventID.OnBarrierLower, thresholdCount);

			if (thresholdCount < thresholds.Length - 1)
				thresholdCount++;
		}
    }

	public int CurrentHealth()
	{
		return health;
	}
}
