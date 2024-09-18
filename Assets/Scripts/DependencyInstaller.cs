using UnityEngine;
using Zenject;

public class DependencyInstaller : MonoInstaller
{
    [SerializeField] private ObjectPool _objectPool;
    [SerializeField] private GameManager _gameManager;

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

        Container.Bind<RotateState>().AsTransient();
        Container.Bind<MoveState>().AsTransient();
    }
}