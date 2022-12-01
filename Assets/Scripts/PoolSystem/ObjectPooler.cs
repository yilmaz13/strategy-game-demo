using System;
using System.Collections.Generic;
using System.Linq;
using Builds;
using UnityEngine;

public sealed class ObjectPooler : MonoBehaviour
{
    private static ObjectPooler _instance = null;
    public static ObjectPooler Instance => _instance;

    #region Variables

    private PoolPack _objectPool;
    public List<Pool> allPools = new List<Pool>();

    #endregion

    #region Unity Methods

    private void Awake()
    {
        //singleton pattern
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }

        _instance = this;

        _SetPool();
        _InstantiatePoolObjects(allPools);
    }

    private void _SetPool()
    {
        try
        {
            _objectPool = Resources.Load<PoolPack>("ScriptableObjects/Pool/ObjectPool");
            List<Pool> pools = _objectPool.pools;
            foreach (Pool pool in pools)
            {
                allPools.Add(Pool.CopyOf(pool));
            }
        }
        catch (Exception e)
        {
            Debug.Log("Scene doesnt have a objectPool");
            Console.WriteLine(e);
            throw;
        }
    }

    private void _InstantiatePoolObjects(List<Pool> pools)
    {
        foreach (var pool in pools)
        {
            var poolParent = new GameObject
            {
                name = pool.Prefab.name,
                transform = { parent = transform }
            };
            pool.StartingParent = poolParent.transform;

            for (var i = 0; i < pool.StartingQuantity; i++)
            {
                GameObject obj = Instantiate(pool.Prefab, poolParent.transform);
                obj.name = obj.name.Substring(0, obj.name.Length - 7) + " " + i;
                pool.PooledObjects.Add(obj);
                obj.SetActive(false);
            }
        }
    }

    #endregion

    #region Public Methods

    public GameObject Spawn(string poolName, Vector3 position, Quaternion rotation = new Quaternion(),
        Transform parentTransform = null, RectTransform rectTransform = null)
    {
        // Find the pool that matches the pool name:
        for (var i = 0; i < allPools.Count; i++)
        {
            if (allPools[i].Prefab.name == poolName)
            {
                foreach (var poolObj in allPools[i].PooledObjects.Where(poolObj => !poolObj.activeSelf))
                {
                    poolObj.SetActive(true);
                    if (rectTransform != null)
                    {
                        if (parentTransform)
                            poolObj.transform.SetParent(parentTransform, false);
                    }
                    else
                    {
                        poolObj.transform.localPosition = position;
                        poolObj.transform.localRotation = rotation;
                        // Set parent:
                        if (parentTransform)
                            poolObj.transform.SetParent(parentTransform, false);
                    }

                    poolObj.GetComponent<PoolObject>().PoolSpawn();

                    return poolObj;
                }

                // If there's no game object available then expand the list by creating a new one:
                var spawnObj = Instantiate(allPools[i].Prefab, allPools[i].StartingParent);
                var childCount = allPools[i].StartingParent.childCount;
                spawnObj.name = spawnObj.name.Substring(0, spawnObj.name.Length - 7) + " " + childCount;
                spawnObj.transform.localPosition = position;
                spawnObj.transform.localRotation = rotation;
                allPools[i].PooledObjects.Add(spawnObj);

                spawnObj.GetComponent<PoolObject>().PoolSpawn();
                return spawnObj;
            }

            if (i != allPools.Count - 1) continue;
            Debug.LogError("!!!There's no pool named \"" + poolName);
            return null;
        }

        return null;
    }

    public Build SpawnBuild(string name, Vector3 pos)
    {
        return Instance.Spawn(name, pos, new Quaternion(), transform).GetComponent<Build>();
    }

    #endregion
}