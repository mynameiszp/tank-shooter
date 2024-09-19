using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemySpawnManager : MonoBehaviour
{
    [Inject] private ObjectPool _objectPool;

    [SerializeField] private SpawnManagerScriptableObject _enemySpawnConfig;
    private List<Vector2> _spawnPoints;

    void Start()
    {
        SpawnEnemies();
        _objectPool.OnAllEnemiesDeath += SpawnEnemies;
    }

    private void SpawnEnemies()
    {
        _spawnPoints = new List<Vector2>(_enemySpawnConfig.spawnPoints);
        GameObject enemy;
        while ((enemy = _objectPool.GetEnemyTank()) != null && _spawnPoints.Count != 0)
        {
            InitializeEnemy(enemy);
        }
    }

    private void InitializeEnemy(GameObject enemy)
    {
        enemy.transform.SetPositionAndRotation(GetRandomPosition(), GetRandomRotation());
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
}
