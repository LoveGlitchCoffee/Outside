using UnityEngine;
using System.Collections;

public enum Chapter
{
	ZeroDay = 0,
    FirstNight = 1,
    FirstDay = 2,
    SecondNight = 3,
    SecondDay = 4,
    ThirdNight = 5,
    ThirdDay = 6
}

public class StoryModel {

	Chapter chap;

	

	public StoryModel()
	{
		// search for save game

		chap = 0;
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
