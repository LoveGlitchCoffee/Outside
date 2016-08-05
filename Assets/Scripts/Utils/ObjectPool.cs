using UnityEngine;
using System.Collections.Generic;

public class ObjectPool
{

    private List<GameObject> objectsInPool;
    Vector2 hide;

    public ObjectPool(GameObject prefab, int initialSize, ref Dictionary<GameObject, ObjectPool> instanceLookup)
    {
        objectsInPool = new List<GameObject>(initialSize);

		// maybe not best place to put?
        hide = Camera.main.ViewportToWorldPoint(new Vector3(1, 1) + new Vector3(100, 100));

        for (int i = 0; i < initialSize; i++)
        {
			GameObject newObject = (GameObject) MonoBehaviour.Instantiate(prefab, hide, Quaternion.identity);
			objectsInPool.Add(newObject);
            instanceLookup.Add(newObject, this);
            newObject.SetActive(false);            
        }

    }

    public void ReturnToPool(GameObject gObject)
    {        
        gObject.transform.position = hide;
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
