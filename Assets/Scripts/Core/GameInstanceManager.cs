using UnityEngine;
using System.Collections;

enum GameState
{
    Menu = 0,
    SelectWeapon = 1,
    InGame = 2,
}

public class GameInstanceManager : Singleton<GameInstanceManager>
{
    private bool playing;

    private GameState state; 

    [Header("Selection")]
    public SelectionControl selectionControl;
    public SelectionModel selectionModel;

    [Header("Cameras")]
    public Camera GameCamera;
    public Camera MenuCamera;
    public Camera SelectionCamera;

    [Header("UI")]
    public GameObject GameUI;

    [Header("MVC")]
    public GameModel model;
    public GameControls control;

    void Start()
    {    
        playing = false;

        state = GameState.Menu;

        this.RegisterListener(EventID.SelectWeaponMenu , (sender, param) => ChangeState(GameState.SelectWeapon));

        this.RegisterListener(EventID.OnGameStart, (sender, param) => ChangeState(GameState.InGame));
        this.RegisterListener(EventID.OnGameEnd, (sender, param) => ChangeState(GameState.Menu));
        
        this.RegisterListener(EventID.OnPlayerDie, (sender, param) => StopPlaying());
        
    }

    private void StopPlaying()
    {
        playing = false;
    }

    private void ChangeState(GameState newState)
    {
        switch (state)
        {
            case GameState.Menu:
            {
                MenuCamera.enabled = false;
                break;
            }
            case GameState.SelectWeapon:
            {
                SelectionCamera.enabled = false;                
                break;
            }
            case GameState.InGame:
            {
                GameCamera.enabled = false;
                GameUI.SetActive(false);                
                break;
            }
        }

        state = newState;
        
        switch (state)
        {
            case GameState.Menu:
            {
                MenuCamera.enabled = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            }
            case GameState.SelectWeapon:
            {
                SelectionCamera.enabled = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            }
            case GameState.InGame:
            {
                GameCamera.enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                GameUI.SetActive(true);
                playing = true;
                break;
            }
        }
    }    

    public bool isPlaying()
    {
        return playing;
    }
}