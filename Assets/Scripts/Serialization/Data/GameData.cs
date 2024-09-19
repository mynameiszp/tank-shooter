using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public PlayerData player;
    public List<EnemyData> enemies;

    public GameData()
    {
        player = new PlayerData();
    }
}
