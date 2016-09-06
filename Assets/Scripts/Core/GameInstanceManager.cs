using UnityEngine;
using System.Collections;

enum GameState
{
    Menu = 0,
    SelectWeapon = 1,
    InGame = 2,
    Credits = 3
}

public class GameInstanceManager : Singleton<GameInstanceManager>
{
    private bool playing;
    bool storyMode;

    private GameState state;

    [Header("Selection")]
    public SelectionControl selectionControl;
    public SelectionModel selectionModel;

    [Header("Story")]
    public TextAsset StoryText;
    public StoryView storyView;
    private StoryModel storyModel;

    [Header("Cameras")]
    public Camera GameCamera;
    public Camera MenuCamera;
    public Camera SelectionCamera;

    [Header("UI")]
    public GameObject GameUI;
    public GameObject SelectionUI;

    [Header("MVC")]
    public GameModel model;
    public GameControls control;
    public GameView view;

    void Start()
    {
        storyModel = new StoryModel(StoryText);        

        playing = false;

        state = GameState.Menu;

        this.RegisterListener(EventID.SelectWeaponMenu, (sender, param) => ChangeState(GameState.SelectWeapon));

        this.RegisterListener(EventID.OnGameStart, (sender, param) => ChangeState(GameState.InGame));
        this.RegisterListener(EventID.OnGameEnd, (sender, param) => ChangeState(GameState.Menu));
        this.RegisterListener(EventID.GoToCredits , (sender, param) => ChangeState(GameState.Credits));

        this.RegisterListener(EventID.OnPlayerDie, (sender, param) => StopPlaying());
        this.RegisterListener(EventID.OnPlayerWin , (sender, param) => StopPlaying());

    }


    public void SetStoryMode()
    {
        storyMode = true;
    }

    public bool IsInStory()
    {
        return storyMode;
    }

    // these two functions are redundant, could make storyModel public but security?
    public int GetCurrentChapter()
    {
        return (int)storyModel.GetChapter();
    }

    public void ProgressStory()
    {
        storyModel.ProgressChapter();
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
                    SelectionUI.SetActive(false);
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
                    storyMode = false;
                    break;
                }
            case GameState.SelectWeapon:
                {
                    SelectionUI.SetActive(true);
                    SelectionCamera.enabled = true;

                    if (storyMode)
                        storyView.SetEntry(storyModel.GetCurrentNarration());

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