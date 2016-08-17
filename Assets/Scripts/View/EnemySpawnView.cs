using UnityEngine;
using System.Collections;

public class EnemySpawnView : MonoBehaviour
{

    public GameObject Zombie;
    public Transform grandpa;

	void Start ()
    {
	    this.RegisterListener(EventID.OnSpawnEnemy, (sender, param) => SpawnEnemy((Vector3) param));
	}

    public void SpawnEnemy(Vector3 location)
    {        
        var zombie = PoolManager.Instance.GetFromPool(Zombie).GetComponent<ZombieBehaviour>();        
        zombie.transform.position = location;
        zombie.SetUp();        
    }
	    
}
