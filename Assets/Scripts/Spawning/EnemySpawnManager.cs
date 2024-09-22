using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class EnemySpawnManager : MonoBehaviour
{
    public Action OnEnemyDataEmpty;

    [Inject] private readonly ObjectPool _objectPool;
    [Inject] private readonly ISpawnStrategy _spawningStrategy;

    private List<Vector2> _spawnPoints;

    void Awake()
    {
        OnEnemyDataEmpty += SpawnEnemies;
        _objectPool.OnAllEnemiesDeath += SpawnEnemies;
    }

    private void SpawnEnemies()
    {
        _spawnPoints = new List<Vector2>(_spawningStrategy.GetSpawnPoints());
        GameObject enemy;
        while ((enemy = _objectPool.GetEnemyTank()) != null && _spawnPoints.Count != 0)
        {
            InitializeEnemy(enemy, GetRandomPosition(), GetRandomRotation());
        }
    }

    public void InitializeEnemy(GameObject enemy, Vector3 position, Quaternion rotation)
    {
        enemy.transform.SetPositionAndRotation(position, rotation);
        enemy.SetActive(true);
        if (enemy.TryGetComponent(out EnemyTank tank))
        {
            tank.OnDestroy += DespawnEnemy;
        }
    }

    private void DespawnEnemy(GameObject enemy)
    {
        enemy.GetComponent<EnemyTank>().OnDestroy -= DespawnEnemy;
        _objectPool.DespawnEnemyTank(enemy);
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
