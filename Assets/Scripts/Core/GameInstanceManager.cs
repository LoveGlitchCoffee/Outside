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
    public SelectionView selectionView;

    [Header("Story")]
    public TextAsset StoryText;
    public StoryView storyView;
    private StoryModel storyModel;

    [Header("Cameras")]
    public Camera GameCamera;
    public Camera MenuCamera;
    public Camera SelectionCamera;

    [Header("Fading")]
    public SpriteRenderer GameFade;
    public SpriteRenderer MenuFade;
    public SpriteRenderer SelectionFade;

    [Header("UI")]
    public GameObject GameUI;
    public GameObject MenuUI;
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
        this.RegisterListener(EventID.GoToCredits, (sender, param) => ChangeState(GameState.Credits));

        this.RegisterListener(EventID.OnPlayerDie, (sender, param) => StopPlaying());
        this.RegisterListener(EventID.OnPlayerWin, (sender, param) => StopPlaying());

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
        StartCoroutine(ChangeStateCoroutine(newState));
    }

    private IEnumerator ChangeStateCoroutine(GameState newState)
    {
        switch (state)
        {
            case GameState.Menu:
                {
                    MenuUI.GetComponent<CanvasGroup>().interactable = false;
                    yield return StartCoroutine(Fade(MenuFade, MenuFade.color, Color.black));
                    MenuUI.SetActive(false);
                    MenuCamera.enabled = false;
                    break;
                }
            case GameState.SelectWeapon:
                {
                    SelectionUI.GetComponent<CanvasGroup>().interactable = false;
                    yield return StartCoroutine(Fade(SelectionFade, SelectionFade.color, Color.black));
                    SelectionUI.SetActive(false);
                    SelectionCamera.enabled = false;
                    break;
                }
            case GameState.InGame:
                {
                    // Game doesn't really need, may delete
                    GameUI.GetComponent<CanvasGroup>().interactable = false;
                    yield return StartCoroutine(Fade(GameFade, GameFade.color, Color.black));
                    GameUI.SetActive(false);
                    GameCamera.enabled = false;
                    break;
                }
        }

        state = newState;

        Color clear = new Color(0, 0, 0, 0);

        switch (state)
        {
            case GameState.Menu:
                {
                    MenuCamera.enabled = true;                    
                    yield return StartCoroutine(Fade(MenuFade, MenuFade.color, clear));
                    MenuUI.SetActive(true);
                    MenuUI.GetComponent<CanvasGroup>().interactable = true;

                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    storyMode = false;
                    break;
                }
            case GameState.SelectWeapon:
                {
                    SelectionCamera.enabled = true;
                    
                    yield return StartCoroutine(Fade(SelectionFade, SelectionFade.color, clear));
                    SelectionUI.SetActive(true);
                    SelectionUI.GetComponent<CanvasGroup>().interactable = true;


                    if (storyMode)
                    {
                        storyView.SetEntry(storyModel.GetCurrentNarration());
                        selectionView.Unlock(storyModel.GetChapter());
                    }

                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    break;
                }
            case GameState.InGame:
                {
                    GameCamera.enabled = true;
                                        
                    yield return StartCoroutine(Fade(GameFade, GameFade.color, clear));
                    GameUI.SetActive(true);
                    GameUI.GetComponent<CanvasGroup>().interactable = true;

                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    playing = true;                    
                    break;
                }
        }
    }

    // arguably in some other class but refactor later

    IEnumerator Fade(SpriteRenderer fade, Color start, Color end)
    {
        WaitForSeconds wait = new WaitForSeconds(0);
        float delta = 0;

        while (fade.color.a != end.a)
        {
            fade.color = Color.Lerp(start, end, delta);
            delta += 0.05f;
            yield return wait;
        }
    }

    public bool isPlaying()
    {
        return playing;
    }
}