using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public PlayerData player;
    public List<EnemyData> enemies;
    public int aliveEnemies;

    public GameData()
    {
        player = new PlayerData();
        enemies = new List<EnemyData>();
        aliveEnemies = 0;
    }

    public EnemyData CreateEnemy()
    {
        var enemy = new EnemyData();
        enemies.Add(enemy);
        return enemy;
    }
}
