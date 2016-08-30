using UnityEngine;
using System.Collections;

public class GameModel : MonoBehaviour
{
    public EnemySpawnerModel enemy;
    public AudioModel audio;

    public SpecialModel special;
    public WeaponModel weapon;
    
    StoryModel story;

    // could be changed to something else later if need
    public Transform Grandpa;

    int score;

    void Start()
    {        
        story = new StoryModel();        

        this.RegisterListener(EventID.OnEnemyDie, (sender, param) => UpdateScore());
        this.RegisterListener(EventID.OnGameEnd, (sender, param) => ResetScore());
    }

    public Chapter CurrentStoryChapter()
    {
        return story.GetChapter();
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
