using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Zenject;

public class PlayerSpawnManager : MonoBehaviour
{
    public event Action<GameObject> OnPlayerSpawned;
    public PlayerTank PlayerTank => _playerTank;

    [Inject] private readonly DiContainer _container;
    [Inject] private readonly ISpawnStrategy _spawningStrategy;
    [Inject] private readonly AvailableAreaDetector _availableAreaDetector;

    [SerializeField] private PlayerTank _playerPrefab;
    [SerializeField] private Vector3 _initialPosition;
    [SerializeField] private float _timeBeforeRespawn;

    private PlayerTank _playerTank;
    private float _timebeforeRetry = 1f;

    private void Awake()
    {
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
        Vector2 spawnPosition = GetPosition();

        while (!_availableAreaDetector.IsAreaAvailable(spawnPosition))
        {
            Debug.LogWarning("No available spawn points, retrying...");
            yield return new WaitForSeconds(_timebeforeRetry); 
            spawnPosition = GetPosition();
        }

        InitializePlayer(spawnPosition);
    }

    private void ResetPlayerInput()
    {
        if (_playerTank.TryGetComponent(out PlayerController playerController))
        {
            playerController.ResetInput();
        }
    }
    private Vector2 GetPosition()
    {
        List<Vector2> availableSpawnPoints = _spawningStrategy.GetSpawnPoints();
        Vector2 pickedPosition = GetRandomPosition(availableSpawnPoints);

        while (!_availableAreaDetector.IsAreaAvailable(pickedPosition) && availableSpawnPoints.Count > 1)
        {
            availableSpawnPoints.Remove(pickedPosition);
            pickedPosition = GetRandomPosition(availableSpawnPoints);
        }
        return pickedPosition;
    }

    private Vector2 GetRandomPosition(List<Vector2> positions)
    {
        int index = Random.Range(0, positions.Count);
        Vector2 pickedPosition = positions[index];
        return pickedPosition;
    }
}
