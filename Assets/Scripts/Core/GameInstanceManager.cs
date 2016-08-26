using UnityEngine;
using System.Collections;

public class GameInstanceManager : Singleton<GameInstanceManager>
{
    private bool playing;

    [Header("Cameras")]
    public Camera GameCamera;
    public Camera MenuCamera;

    [Header("UI")]
    public GameObject GameUI;

    public GameModel model;        

    void Start()
    {
        playing = false;
        this.RegisterListener(EventID.OnGameStart, (sender, param) => StartGame());
        this.RegisterListener(EventID.OnGameEnd, (sender, param) => StopGame());
        this.RegisterListener(EventID.OnPlayerDie , (sender, param) => StopPlaying());        
    }    

    private void StopPlaying()
    {
        playing = false;
    } 

    public void StartGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        MenuCamera.enabled = false;
        GameCamera.enabled = true;
        GameUI.SetActive(true);
        playing = true;        
    }    

    public void StopGame()
    {     
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameCamera.enabled = false;
        MenuCamera.enabled = true;
        GameUI.SetActive(false);
    }    

    public bool isPlaying()
    {
        return playing;
    }
}