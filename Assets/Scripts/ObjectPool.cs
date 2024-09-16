using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private ObjectPoolConfig _bulletPoolConfig;
    [SerializeField] private ObjectPoolConfig _tankPoolConfig;

    void Start()
    {
        InitializeList(_bulletPoolConfig);
        InitializeList(_tankPoolConfig);
    }

    public GameObject GetBullet()
    {
        return GetPooledObject(_bulletPoolConfig);
    }    
    
    public GameObject GetTank()
    {
        return GetPooledObject(_tankPoolConfig);
    }

    private void InitializeList(ObjectPoolConfig config)
    {
        config.pooledObjects = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < config.poolCapacity; i++)
        {
            tmp = Instantiate(config.objectPrefab);
            tmp.SetActive(false);
            config.pooledObjects.Add(tmp);
        }
    }

    private GameObject GetPooledObject(ObjectPoolConfig config)
    {
        for (int i = 0; i < config.poolCapacity; i++)
        {
            if (!config.pooledObjects[i].activeInHierarchy)
            {
                return config.pooledObjects[i];
            }
        }
        return null;
    }
}
