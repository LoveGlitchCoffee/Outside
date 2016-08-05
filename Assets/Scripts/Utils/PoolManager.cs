using UnityEngine;
using System.Collections.Generic;

public class PoolManager : Singleton<PoolManager>
{
    public GameObject[] objectPrefabs;
    public int initialSize;

    private Dictionary<GameObject, ObjectPool> prefabLookup;
    private Dictionary<GameObject, ObjectPool> instanceLookup;    

    void Start()
    {
        prefabLookup = new Dictionary<GameObject, ObjectPool>();
        instanceLookup = new Dictionary<GameObject, ObjectPool>();

        for (int i = 0; i < objectPrefabs.Length; i++)
        {
            if (objectPrefabs[i] == null)
                continue;
            
            prefabLookup.Add(objectPrefabs[i], new ObjectPool(objectPrefabs[i], initialSize, ref instanceLookup));            
        }
    }

    public GameObject GetFromPool(GameObject gObject)
    {
        GameObject rObject = null;

        try
        {
            ObjectPool gPool = prefabLookup[gObject];
            rObject = gPool.GetFromPool();
            rObject.SetActive(true);
        }
        catch (System.Exception)
        {
            rObject = Instantiate(gObject);
            instanceLookup.Add(rObject, prefabLookup[gObject]);
            //Debug.Log("created one");
        }

        return rObject;
    }

    public void ReturnToPool(GameObject gObject)
    {
        ObjectPool gPool = instanceLookup[gObject];        
		gPool.ReturnToPool(gObject);
    }
}
