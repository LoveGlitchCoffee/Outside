using UnityEngine;
using System.Collections;

public class MenuControl : GameElement {

    // story mode
    public void StartGame()
    {
        if ((int)GameManager.model.CurrentStoryChapter() % 2 == 0)
        {
            this.PostEvent(EventID.SelectWeaponMenu);
        }
    }



}
