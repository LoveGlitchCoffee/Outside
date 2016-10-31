using UnityEngine;
using System.Collections;

public class PauseControl : MonoBehaviour {

	public void ReturnToMenu()
	{
		this.PostEvent(EventID.OnGameEnd);
	}

}
