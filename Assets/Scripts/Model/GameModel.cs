using UnityEngine;
using System.Collections;

public class GameModel : MonoBehaviour
{
    public EnemySpawnerModel enemy;
    public AudioModel audio;

    public SpecialModel special;

    int score;

    void Start()
    {        
        this.RegisterListener(EventID.OnEnemyDie, (sender, param) => UpdateScore());
        this.RegisterListener(EventID.OnGameEnd, (sender, param) => ResetScore());
    }

    private void UpdateScore()
    {
        score++;
        this.PostEvent(EventID.OnUpdateScore, score);
    }

    private void ResetScore()
    {
        score = 0;
        this.PostEvent(EventID.OnUpdateScore, score);
    }
}
