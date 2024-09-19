using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerSpawnManager : MonoBehaviour
{
    public event Action<GameObject> OnPlayerSpawned;

    [SerializeField] private SpawnManagerScriptableObject _playerSpawnConfig;
    [SerializeField] private PlayerTank _playerPrefab;
    [SerializeField] private Vector3 _initialPosition;
    [SerializeField] private float _timeBeforeRespawn;

    [Inject] private DiContainer _container;

    private PlayerTank _playerTank;
    private List<Vector2> _spawnPoints;

    private void Start()
    {
        _spawnPoints = new List<Vector2>(_playerSpawnConfig.spawnPoints);
        SpawnPlayer();
        _playerTank.OnDestroy += HandleDeath;
    }

    private void SpawnPlayer()
    {
        var player = _container.InstantiatePrefab(_playerPrefab);
        _playerTank = player.GetComponent<PlayerTank>();
        InitializePlayer(_initialPosition);
        OnPlayerSpawned?.Invoke(player);
    }

    private void InitializePlayer(Vector3 position)
    {
        _playerTank.transform.position = position;
        _playerTank.gameObject.SetActive(true);
    }

    private void HandleDeath()
    {
        _playerTank.gameObject.SetActive(false);
        ResetPlayerInput();
        StartCoroutine(RespawnPlayer());
    }

    private IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(_timeBeforeRespawn);
        InitializePlayer(GetRandomPosition());
    }

    private void ResetPlayerInput()
    {
        if (_playerTank.TryGetComponent(out PlayerController playerController))
        {
            playerController.ResetInput();
        }
    }

    private Vector2 GetRandomPosition()
    {
        int index = UnityEngine.Random.Range(0, _spawnPoints.Count);
        Vector2 pickedPosition = _spawnPoints[index];
        return pickedPosition;
    }
}
