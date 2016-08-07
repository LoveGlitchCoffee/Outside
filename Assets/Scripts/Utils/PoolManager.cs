using System;
using UnityEngine;
using System.Collections.Generic;

public class PoolManager : Singleton<PoolManager>
{
    public GameObject[] prefabObjects;
    public int initialSize;

    private Dictionary<GameObject, ObjectPool> prefabLookup;
    private Dictionary<GameObject, ObjectPool> instanceLookup;

    void Start()
    {
        prefabLookup = new Dictionary<GameObject, ObjectPool>();
        instanceLookup = new Dictionary<GameObject, ObjectPool>();

        for (int i = 0; i < prefabObjects.Length; i++)
        {
            if (prefabObjects[i] == null)
                continue;

            prefabLookup.Add(prefabObjects[i], new ObjectPool(prefabObjects[i], initialSize, ref instanceLookup));
        }
    }

    public GameObject GetFromPool(GameObject gObject)
    {
        if (prefabLookup[gObject] == null)
        {
            prefabLookup.Add(gObject, new ObjectPool(gObject, initialSize, ref instanceLookup));
        }

        GameObject rObject = null;

        try
        {
            ObjectPool gPool = prefabLookup[gObject];
            rObject = gPool.GetFromPool();
        }
        catch (Exception e)
        {
            if (e is KeyNotFoundException)
            {
                prefabLookup.Add(gObject, new ObjectPool(gObject, initialSize, ref instanceLookup));

                ObjectPool gPool = prefabLookup[gObject];
                rObject = gPool.GetFromPool();
            }
        }

        if (rObject == null)
        {
            rObject = Instantiate(gObject);
            instanceLookup.Add(rObject, prefabLookup[gObject]);
        }

        rObject.SetActive(true);

        return rObject;
    }

    public GameObject GetFromPool(GameObject gObject, Vector3 position, Quaternion rotation)
    {
        if (prefabLookup[gObject] == null)
        {
            prefabLookup.Add(gObject, new ObjectPool(gObject, initialSize, ref instanceLookup));
        }

        GameObject rObject = null;

        try
        {
            ObjectPool gPool = prefabLookup[gObject];
            rObject = gPool.GetFromPool();
        }
        catch (Exception e)
        {
            if (e is KeyNotFoundException)
            {
                prefabLookup.Add(gObject, new ObjectPool(gObject, initialSize, ref instanceLookup));

                ObjectPool gPool = prefabLookup[gObject];
                rObject = gPool.GetFromPool();
            }
        }

        if (rObject == null)
        {
            rObject = Instantiate(gObject);
            instanceLookup.Add(rObject, prefabLookup[gObject]);
        }


        rObject.transform.position = position;
        rObject.transform.rotation = rotation;

        rObject.SetActive(true);

        return rObject;
    }

    public void ReturnToPool(GameObject gObject)
    {
        ObjectPool gPool = null;

        try
        {
            gPool = instanceLookup[gObject];
        }
        catch (Exception)
        {
            Debug.Log(gObject + " has no pool");
        }


        gPool.ReturnToPool(gObject);
    }
}