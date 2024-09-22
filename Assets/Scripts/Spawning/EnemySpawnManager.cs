using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class EnemySpawnManager : MonoBehaviour
{
    public Action OnEnemyDataEmpty;
    public int SpawnedEnemiesCount { get; set; }

    [Inject] private readonly ObjectPool _objectPool;
    [Inject] private readonly ISpawnStrategy _spawningStrategy;

    private List<Vector2> _spawnPoints;
    private int _aliveEnemiesCount;

    void Awake()
    {
        _spawnPoints = _spawningStrategy.GetSpawnPoints();
        OnEnemyDataEmpty += SpawnEnemies;
        _objectPool.OnAllEnemiesDeath += SpawnEnemies;
    }

    public void InitializeEnemy(GameObject enemy, Vector3 position, Quaternion rotation)
    {
        enemy.transform.SetPositionAndRotation(position, rotation);
        enemy.SetActive(true);
        if (enemy.TryGetComponent(out EnemyTank tank))
        {
            tank.OnDestroy += DespawnEnemy;
        }
        _aliveEnemiesCount++;
    }
    
    public int GetAliveEnemiesCount()
    {
        return _aliveEnemiesCount;
    }

    public int GetTotalEnemiesCount()
    {
        var pointsCount = _spawnPoints.Count;
        var poolSize = _objectPool.GetEnemiesPoolSize();
        return pointsCount >= poolSize ? poolSize : pointsCount;
    }

    private void SpawnEnemies()
    {
        _spawnPoints = _spawningStrategy.GetSpawnPoints();
        GameObject enemy;
        while ((enemy = _objectPool.GetEnemyTank()) != null && _spawnPoints.Count != 0)
        {
            InitializeEnemy(enemy, GetRandomPosition(), GetRandomRotation());
        }
    }

    private void DespawnEnemy(GameObject enemy)
    {
        enemy.GetComponent<EnemyTank>().OnDestroy -= DespawnEnemy;
        _objectPool.DespawnEnemyTank(enemy);
        _aliveEnemiesCount--;
    }

    private Vector2 GetRandomPosition()
    {
        int index = Random.Range(0, _spawnPoints.Count);
        Vector2 pickedPosition = _spawnPoints[index];
        _spawnPoints.RemoveAt(index);
        return pickedPosition;
    }
    private Quaternion GetRandomRotation()
    {
        return Quaternion.Euler(0, 0, Random.Range(0f, 360f));
    }
}
