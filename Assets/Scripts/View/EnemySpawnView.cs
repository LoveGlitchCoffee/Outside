using UnityEngine;
using System.Collections;

public class EnemySpawnView : GameElement
{

    public GameObject Zombie;
    public Transform grandpa;

	void Start ()
    {
	    this.RegisterListener(EventID.OnSpawnEnemy, (sender, param) => SpawnEnemy((Vector3) param));
	}

    public void SpawnEnemy(Vector3 location)
    {        
        var zombie = PoolManager.Instance.spawnObject(Zombie).GetComponent<ZombieBehaviour>();        
        zombie.transform.position = location;
        zombie.SetUp(GameManager.model.enemy.IsFast());        
    }
	    
}
