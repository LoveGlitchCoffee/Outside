using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreView : MonoBehaviour
{
    
    private Text score;

	void Start ()
	{
	    score = GetComponent<Text>();

        this.RegisterListener(EventID.OnUpdateScore, (sender, param) => UpdateScore((int) param));
	}

    private void UpdateScore(int newScore)
    {
        score.text = newScore.ToString();
    }
}
