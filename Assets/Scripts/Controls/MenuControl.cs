using UnityEngine;
using System.Collections;

public class MenuControl : MonoBehaviour {


    public void StartGame()
    {        
        this.PostEvent(EventID.OnGameStart);
    }

}
