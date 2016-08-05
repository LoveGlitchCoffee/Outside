using UnityEngine;
using System.Collections;

public class GameInstanceManager : Singleton<GameInstanceManager>
{
    private bool playing;

    public Camera GameCamera;
    public Camera MenuCamera;

    public GameModel model;    

    void Start()
    {
        playing = false;
        this.RegisterListener(EventID.OnGameStart, (sender, param) => StartGame());
        this.RegisterListener(EventID.OnGameEnd, (sender, param) => StopGame());        
    }    

    public void StartGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        MenuCamera.enabled = false;
        GameCamera.enabled = true;
        playing = true;        
    }    

    public void StopGame()
    {     
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameCamera.enabled = false;
        MenuCamera.enabled = true;
        playing = false;
    }    

    public bool isPlaying()
    {
        return playing;
    }
}