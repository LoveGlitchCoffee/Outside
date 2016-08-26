using UnityEngine;
using System.Collections.Generic;

public class ObjectPool
{

    private List<GameObject> objectsInPool;

    public ObjectPool(GameObject prefab, int initialSize, ref Dictionary<GameObject, ObjectPool> instanceLookup)
    {
        objectsInPool = new List<GameObject>(initialSize);

        for (int i = 0; i < initialSize; i++)
        {
			GameObject newObject = (GameObject) MonoBehaviour.Instantiate(prefab, PoolManager.Instance.transform.position, Quaternion.identity);
			objectsInPool.Add(newObject);
            instanceLookup.Add(newObject, this);
            newObject.SetActive(false);            
        }

    }

    public void ReturnToPool(GameObject gObject)
    {        
        gObject.SetActive(false);
        objectsInPool.Add(gObject);
    }

    public GameObject GetFromPool()
    {
        GameObject returnedObject = null;

        if (objectsInPool.Count > 0)
        {
            returnedObject = objectsInPool[0];
            objectsInPool.RemoveAt(0);
        }      

        return returnedObject;
    }
}
