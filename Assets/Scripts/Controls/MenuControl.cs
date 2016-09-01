using UnityEngine;
using System.Collections;

public class MenuControl : GameElement
{

    // story mode
    public void StartStory()
    {
        GameManager.SetStoryMode();
        this.PostEvent(EventID.SelectWeaponMenu);
    }



}
