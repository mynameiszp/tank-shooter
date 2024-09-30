using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ObjectPool : MonoBehaviour
{
    public event Action OnAllEnemiesDeath;
    public List<GameObject> ActiveEnemies => _activeEnemies;

    [SerializeField] private ObjectPoolConfig _playerBulletPoolConfig;
    [SerializeField] private ObjectPoolConfig _enemyPoolConfig;

    [Inject] private DiContainer _container;

    private List<GameObject> _activeEnemies = new List<GameObject>();

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
        var enemy = GetPooledObject(_enemyPoolConfig);
        if (enemy != null)
        {
            if (!_activeEnemies.Contains(enemy)) 
            {
                _activeEnemies.Add(enemy); 
            }
        }
        return enemy;
    }

    public int GetEnemiesPoolSize()
    {
        return _enemyPoolConfig.poolCapacity;
    }

    public void DespawnEnemyTank(GameObject enemy)
    {
        enemy.SetActive(false);
        _activeEnemies.Remove(enemy);
        if (IsPoolNotActive(_enemyPoolConfig))
        {
            OnAllEnemiesDeath?.Invoke();
        }
    }

    public void DespawnAllEnemyTanks()
    {
        foreach (var enemy in _activeEnemies)
        {
            enemy.SetActive(false);
        }
        _activeEnemies.Clear();
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
