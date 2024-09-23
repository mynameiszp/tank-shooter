using UnityEngine;
using Zenject;

public class PlayerDataPersistence : MonoBehaviour, IDataPersistence
{
    [Inject] private PlayerSpawnManager _playerSpawner;
    private Transform _playerTransform;

    private void Start()
    {
        _playerTransform = _playerSpawner.PlayerTank.transform;
    }
    public void LoadData(GameData gameData)
    {
        _playerTransform.position = gameData.player.position;
        _playerTransform.rotation = gameData.player.rotation;
    }

    public void SaveData(GameData gameData)
    {
        gameData.player.position = _playerTransform.position;
        gameData.player.rotation = _playerTransform.rotation;
    }
}
