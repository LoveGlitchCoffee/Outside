using UnityEngine;
using System.Collections;

public enum Chapter
{
	NightZero = 0,
	NightOne = 1, 
	NightTwo = 2,
	NightThree = 3,
	NightFour = 4,
	EndGame = 5
}

public class StoryModel {

	Chapter chap;

	TextAsset story;
	
	string[] narrations;

	public StoryModel(TextAsset story)
	{
		this.story = story;
		
		LoadText();
		// search for save game		

		chap = Chapter.NightZero;
	}

	private void LoadText()
	{
		string narrationText = story.text;
		narrations = narrationText.Split('/');
	}

	public string GetCurrentNarration()
	{
		return narrations[(int)chap];
	}

	public Chapter GetChapter()
	{
		return chap;
	}

	public void ProgressChapter()
	{
		int current = (int)chap;
		current++;
		chap = (Chapter)current;
	}
}
