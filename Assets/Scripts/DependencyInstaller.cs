using UnityEngine;
using Zenject;

public class DependencyInstaller : MonoInstaller
{
    [SerializeField] private ObjectPool _objectPool;
    [SerializeField] private PlayerSpawnManager _playerSpawnManager;
    [SerializeField] private EnemySpawnManager _enemySpawnManager;
    [SerializeField] private CameraManager _cameraManager;
    [SerializeField] private DataPersistenceManager _dataPersistenceManager;
    [SerializeField] private AvailableAreaDetector _areaDetector;

    [SerializeField] private TankConfig _playerTankConfig;
    [SerializeField] private SpawnManagerScriptableObject _playerSpawnConfig;
    [SerializeField] private SpawnManagerScriptableObject _enemySpawnConfig;

    public override void InstallBindings()
    {
        InitializeManagers(); 
        
        Container.Bind<ObjectPool>()
            .FromInstance(_objectPool)
            .AsSingle();         
        
        Container.Bind<AvailableAreaDetector>()
            .FromInstance(_areaDetector)
            .AsSingle();        
        
        Container.Bind<ITankAI>()
            .To<RandomMovementAI>()
            .AsTransient();

        Container.Bind<FileDataHandler>()
            .To<JsonFileDataHandler>()
            .AsTransient()
            .WithArguments(Application.persistentDataPath, _dataPersistenceManager.FileName, _dataPersistenceManager.UseEncryption);

        Container.Bind<IEncryptor>()
            .To<XorEncryptor>()
            .AsTransient();
        
        Container.Bind<IMovementStrategy>()
            .To<BasicTankMovement>()
            .AsTransient()
            .WithArguments(_playerTankConfig.moveSpeed, _playerTankConfig.rotationSpeed);

        Container.Bind<RotateState>().AsTransient();
        Container.Bind<MoveState>().AsTransient();

        Container.QueueForInject(_cameraManager);

        _playerSpawnManager.OnPlayerSpawned += _cameraManager.SetCameraTarget;

        InitializeScriptableObjects();
    }

    private void InitializeScriptableObjects()
    {
        Container.Bind<ISpawnStrategy>()
            .To<FixedPointsSpawnStrategy>()
            .FromMethod(context => new FixedPointsSpawnStrategy(_playerSpawnConfig.spawnPoints))
            .WhenInjectedInto<PlayerSpawnManager>();

        Container.Bind<ISpawnStrategy>()
            .To<FixedPointsSpawnStrategy>()
            .FromMethod(context => new FixedPointsSpawnStrategy(_enemySpawnConfig.spawnPoints))
            .WhenInjectedInto<EnemySpawnManager>();
    }

    private void InitializeManagers()
    {
        Container.Bind<PlayerSpawnManager>()
            .FromInstance(_playerSpawnManager)
            .AsSingle();

        Container.Bind<EnemySpawnManager>()
            .FromInstance(_enemySpawnManager)
            .AsSingle();

        Container.Bind<CameraManager>()
            .FromInstance(_cameraManager)
            .AsSingle();
    }
}