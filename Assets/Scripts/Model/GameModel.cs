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
    int enemyCount = 0;
    int currentCap;

    void Start()
    {
        enemyAmounts = new int[] { 20, 20, 50, 50, 70 };

        this.RegisterListener(EventID.OnGameStart, (sender, param) => SetEnemyCap()); // doens't matter even in endless
        this.RegisterListener(EventID.OnEnemyDie, (sender, param) => UpdateScore());
        this.RegisterListener(EventID.OnSpawnEnemy, (sender, param) => UpdateEnemyCount());
        this.RegisterListener(EventID.OnGameEnd, (sender, param) => ResetScore());
        this.RegisterListener(EventID.OnPlayerWin, (sender, param) => ProgressStory());
    }

    private void SetEnemyCap()
    {
        currentCap = enemyAmounts[GameManager.GetCurrentChapter()];
        Debug.Log("enemy cap " + currentCap);
    }
    public int GetEnemyCount()
    {
        return enemyCount;
    }

    public int GetEnemyCurrentCap()
    {
        return currentCap;
    }

    private void UpdateEnemyCount()
    {
        // unecessary check but might help
        if (!GameManager.IsInStory())
            return;

        enemyCount++;
        Debug.Log("spawned " + enemyCount);
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
        if (GameManager.IsInStory())
        {
            enemyCount = 0;
        }

        score = 0;
        this.PostEvent(EventID.OnUpdateScore, score);
    }

    private void ProgressStory()
    {
        ResetScore();
        Debug.Log("moving on next story");
        StartCoroutine(WaitToFinish());
    }

    private IEnumerator WaitToFinish()
    {
        GameManager.ProgressStory();

        yield return new WaitForSeconds(3);

        this.PostEvent(EventID.SelectWeaponMenu);
    }
}
