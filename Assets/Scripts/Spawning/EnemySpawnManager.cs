using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using Zenject;
using Random = UnityEngine.Random;

public class EnemySpawnManager : MonoBehaviour
{
    public Action OnEnemyDataEmpty;
    public int SpawnedEnemiesCount { get; set; }

    [Inject] private readonly ObjectPool _objectPool;
    [Inject] private readonly ISpawnStrategy _spawningStrategy;
    [Inject] private readonly AvailableAreaDetector _availableAreaDetector;

    private List<Vector2> _uniqueSpawnPoints;
    private List<Vector2> _initialSpawnPoints;
    private int _aliveEnemiesCount;

    private float _timebeforeRetry = 1f;
    private const string ENEMY_LAYER = GameConstants.ENEMY_LAYER_NAME;
    private const string PLAYER_LAYER = GameConstants.PLAYER_LAYER_NAME;

    void Awake()
    {
        _uniqueSpawnPoints = _spawningStrategy.GetSpawnPoints();
        _initialSpawnPoints = _spawningStrategy.GetSpawnPoints();
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
            tank.OnEnemiesCollision += RespawnEnemyRandomly;
        }
        _aliveEnemiesCount++;
    }
    
    public int GetAliveEnemiesCount()
    {
        return _aliveEnemiesCount;
    }

    public int GetTotalEnemiesCount()
    {
        var pointsCount = _initialSpawnPoints.Count;
        var poolSize = _objectPool.GetEnemiesPoolSize();
        return pointsCount >= poolSize ? poolSize : pointsCount;
    }

    private void SpawnEnemies()
    {
        _uniqueSpawnPoints = _spawningStrategy.GetSpawnPoints();
        GameObject enemy;
        while ((enemy = _objectPool.GetEnemyTank()) != null && _uniqueSpawnPoints.Count != 0)
        {
            InitializeEnemy(enemy, GetUniqueRandomPosition(), GetRandomRotation());
        }
    }

    private void DespawnEnemy(GameObject enemy)
    {
        enemy.GetComponent<EnemyTank>().OnDestroy -= DespawnEnemy;
        _objectPool.DespawnEnemyTank(enemy);
        _aliveEnemiesCount--;
    }
    private void RespawnEnemyRandomly(GameObject enemy)
    {
        DespawnEnemy(enemy);
        SpawnEnemyRandomly();
    }
    private void SpawnEnemyRandomly()
    {
        GameObject enemy = _objectPool.GetEnemyTank();
        if (enemy != null)
        {
            InitializeEnemy(enemy, GetAvailablePosition(), GetRandomRotation());
        }
    }
    private Vector2 GetUniqueRandomPosition()
    {
        int index = Random.Range(0, _uniqueSpawnPoints.Count);
        Vector2 pickedPosition = _uniqueSpawnPoints[index];
        _uniqueSpawnPoints.RemoveAt(index);
        return pickedPosition;
    }
    private Vector2 GetAvailablePosition()
    {
        Vector2 spawnPosition = GetRandomPosition();
        while (!_availableAreaDetector.IsAreaAvailable(spawnPosition, LayerMask.NameToLayer(ENEMY_LAYER)) &&
            !_availableAreaDetector.IsAreaAvailable(spawnPosition, LayerMask.NameToLayer(PLAYER_LAYER)))
        {
            Debug.Log("Changing position");
            StartCoroutine(WaitBeforeRetry(_timebeforeRetry));
            spawnPosition = GetRandomPosition();
        }
        return spawnPosition;
    }
    private IEnumerator WaitBeforeRetry(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    private Vector2 GetRandomPosition()
    {
        var positions = _spawningStrategy.GetSpawnPoints();
        int index = Random.Range(0, positions.Count);
        Vector2 pickedPosition = positions[index];
        return pickedPosition;
    }
    private Quaternion GetRandomRotation()
    {
        return Quaternion.Euler(0, 0, Random.Range(0f, 360f));
    }
}
