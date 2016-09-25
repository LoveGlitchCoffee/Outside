using UnityEngine;
using System.Collections.Generic;

public class ObjectPool
{

    private List<ObjectContainer<GameObject>> objectsInPool;

    public ObjectPool(GameObject prefab, int initialSize, ref Dictionary<GameObject, ObjectPool> instanceLookup)
    {
        objectsInPool = new List<ObjectContainer<GameObject>>(initialSize);

        for (int i = 0; i < initialSize; i++)
        {
			GameObject newObject = (GameObject) MonoBehaviour.Instantiate(prefab, PoolManager.Instance.transform.position, Quaternion.identity);
            newObject.name += i.ToString();

            ObjectContainer<GameObject> objCont = new ObjectContainer<GameObject>();
            objCont.Item = newObject;

			objectsInPool.Add(objCont);
            instanceLookup.Add(objCont, this);
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
