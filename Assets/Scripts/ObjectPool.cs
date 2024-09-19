using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ObjectPool : MonoBehaviour
{
    public event Action OnAllEnemiesDeath;

    [SerializeField] private ObjectPoolConfig _playerBulletPoolConfig;
    [SerializeField] private ObjectPoolConfig _enemyPoolConfig;

    [Inject] private DiContainer _container;

    void Awake()
    {
        InitializeList(_playerBulletPoolConfig);
        InitializeList(_enemyPoolConfig);
    }

    public GameObject GetPlayerBullet()
    {
        return GetPooledObject(_playerBulletPoolConfig);
    }    
    
    public GameObject GetEnemyTank()
    {
        return GetPooledObject(_enemyPoolConfig);
    }

    public void DespawnEnemyTank(GameObject enemy)
    {
        enemy.SetActive(false);
        if (IsPoolNotActive(_enemyPoolConfig))
        {
            OnAllEnemiesDeath?.Invoke();
        }
    }

    private void InitializeList(ObjectPoolConfig config)
    {
        config.pooledObjects = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < config.poolCapacity; i++)
        {
            tmp = _container.InstantiatePrefab(config.objectPrefab, config.parentObject);
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

    private bool IsPoolNotActive(ObjectPoolConfig config)
    {
        foreach (var item in config.pooledObjects)
        {
            if (item.activeInHierarchy)
            {
                return false;
            }
        }
        return true;
    }
}
