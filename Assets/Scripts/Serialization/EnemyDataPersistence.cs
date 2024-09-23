using UnityEngine;
using Zenject;

public class EnemyDataPersistence : MonoBehaviour, IDataPersistence
{
    [Inject] private ObjectPool _objectPool;
    [Inject] private EnemySpawnManager _enemySpawner;

    public void LoadData(GameData gameData)
    {
        _objectPool.DespawnAllEnemyTanks();

        foreach (var enemyData in gameData.enemies)
        {
            GameObject enemy = _objectPool.GetEnemyTank();
            if (enemy != null)
            {
                _enemySpawner.InitializeEnemy(enemy, enemyData.position, enemyData.rotation);
            }
        }

        if (gameData.enemies.Count == 0)
        {
            _enemySpawner.OnEnemyDataEmpty?.Invoke();
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

        gameData.aliveEnemies = _enemySpawner.GetAliveEnemiesCount();
    }
}
