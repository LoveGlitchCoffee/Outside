using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StoryView : MonoBehaviour {

	private Text Entry;

	void Awake()
	{
		Entry = GetComponent<Text>();
	}

	public void SetEntry(string story)
	{
		Entry.text = story;
	}

	
}
