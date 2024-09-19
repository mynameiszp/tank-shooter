using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class EnemySpawnManager : MonoBehaviour, IDataPersistence
{
    public event Action OnEnemyDataEmpty;

    [Inject] private ObjectPool _objectPool;

    [SerializeField] private SpawnManagerScriptableObject _enemySpawnConfig;
    private List<Vector2> _spawnPoints;

    void Awake()
    {
        OnEnemyDataEmpty += SpawnEnemies;
        _objectPool.OnAllEnemiesDeath += SpawnEnemies;
    }

    private void SpawnEnemies()
    {
        _spawnPoints = new List<Vector2>(_enemySpawnConfig.spawnPoints);
        GameObject enemy;
        while ((enemy = _objectPool.GetEnemyTank()) != null && _spawnPoints.Count != 0)
        {
            InitializeEnemy(enemy, GetRandomPosition(), GetRandomRotation());
        }
    }

    private void InitializeEnemy(GameObject enemy, Vector3 position, Quaternion rotation)
    {
        enemy.transform.SetPositionAndRotation(position, rotation);
        enemy.SetActive(true);
        if (enemy.TryGetComponent(out EnemyController controller))
        {
            controller.OnDestroy += DespawnEnemy;
        }
    }

    private void DespawnEnemy(GameObject enemy)
    {
        enemy.GetComponent<EnemyController>().OnDestroy -= DespawnEnemy;
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

    public void LoadData(GameData gameData)
    {
        _objectPool.DespawnAllEnemyTanks();

        foreach (var enemyData in gameData.enemies)
        {
            GameObject enemy = _objectPool.GetEnemyTank();
            if (enemy != null)
            {
                InitializeEnemy(enemy, enemyData.position, enemyData.rotation);
            }
        }

        if (gameData.enemies.Count == 0)
        {
            OnEnemyDataEmpty?.Invoke();
        }
    }

    public void SaveData(GameData gameData)
    {
        gameData.enemies.Clear();

        foreach (var enemy in _objectPool.ActiveEnemies)
        {
            if (enemy.activeInHierarchy)
            {
                Vector3 position = enemy.transform.position;
                Quaternion rotation = enemy.transform.rotation;
                var enemyData = gameData.CreateEnemy();
                enemyData.position = position;
                enemyData.rotation = rotation;
            }
        }
    }
}
