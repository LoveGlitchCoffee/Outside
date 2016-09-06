using UnityEngine;
using System.Collections;

public class GameModel : GameElement
{
    public EnemySpawnerModel enemy;
    public AudioModel audio;

    public SpecialModel special;
    public WeaponModel weapon;

    // could be changed to something else later if need
    public Transform Grandpa;

    // Score for endless
    int score;

    // story mode enemy count
    int[] enemyAmounts;
    float[] enemyFast;
    int currentCap;

    bool ready;

    void Start()
    {
        enemyAmounts = new int[] { 5, 20, 50, 50, 70 };
        enemyFast = new float[] { 0f, 0.3f, 0.3f, 0.5f, 0.5f };

        this.RegisterListener(EventID.OnGameStart, (sender, param) => SetEnemyCap()); // doens't matter even in endless
        this.RegisterListener(EventID.OnEnemyDie, (sender, param) => UpdateScore());
        this.RegisterListener(EventID.OnGameEnd, (sender, param) => ResetScore());
        this.RegisterListener(EventID.OnGameProceed , (sender, param) => ResetScore());
        this.RegisterListener(EventID.OnPlayerWin, (sender, param) => ProgressStory());
    }

    private void SetEnemyCap()
    {
        currentCap = enemyAmounts[GameManager.GetCurrentChapter()];
        ready = true;
        Debug.Log("enemy cap " + currentCap);
    }
    

    public int GetEnemyCurrentCap()
    {
        return currentCap;
    }

    public float GetCurrentFast()
    {
        return enemyFast[GameManager.GetCurrentChapter()];
    }

    public bool Set()
    {
        return ready;
    }

    private void UpdateScore()
    {
        score++;
        this.PostEvent(EventID.OnUpdateScore, score);

        if (GameManager.IsInStory() && score == currentCap)
            this.PostEvent(EventID.OnPlayerWin);

    }

    private void ResetScore()
    {
        score = 0;
        this.PostEvent(EventID.OnUpdateScore, score);
    }

    private void ProgressStory()
    {
        ResetScore();
        ready = false;

        Debug.Log("moving on next story");
        StartCoroutine(WaitToFinish());
    }

    private IEnumerator WaitToFinish()
    {
        if (GameManager.GetCurrentChapter() < (int)Chapter.EndGame)
        {
            GameManager.ProgressStory();

            yield return new WaitForSeconds(3);

            this.PostEvent(EventID.SelectWeaponMenu);
        }
        else
        {
            yield return new WaitForSeconds(3);

            this.PostEvent(EventID.GoToCredits);
        }

        this.PostEvent(EventID.OnGameProceed);
    }
}
