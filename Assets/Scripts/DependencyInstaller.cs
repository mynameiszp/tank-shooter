using UnityEngine;
using Zenject;

public class DependencyInstaller : MonoInstaller
{
    [SerializeField] private ObjectPool _objectPool;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private PlayerSpawnManager _playerSpawnManager;
    [SerializeField] private CameraManager _cameraManager;

    public override void InstallBindings()
    {
        Container.Bind<ObjectPool>()
            .FromInstance(_objectPool)
            .AsSingle();        
        
        Container.Bind<GameManager>()
            .FromInstance(_gameManager)
            .AsSingle();

        Container.Bind<ITankAI>()
            .To<RandomMovementAI>()
            .AsTransient();

        Container.Bind<PlayerSpawnManager>()
            .FromInstance(_playerSpawnManager)
            .AsSingle();

        Container.Bind<CameraManager>()
            .FromInstance(_cameraManager)
            .AsSingle();

        Container.Bind<RotateState>().AsTransient();
        Container.Bind<MoveState>().AsTransient();

        Container.QueueForInject(_cameraManager);

        _playerSpawnManager.OnPlayerSpawned += _cameraManager.SetCameraTarget;
    }
}