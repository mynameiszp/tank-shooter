using UnityEngine;
using Zenject;

public class DependencyInstaller : MonoInstaller
{
    [SerializeField] private ObjectPool _objectPool;
    [SerializeField] private StateMachine _stateMachine;

    public override void InstallBindings()
    {
        Container.Bind<ObjectPool>()
            .To<ObjectPool>()
            .FromInstance(_objectPool);

        Container.Bind<StateMachine>()
            .To<StateMachine>()
            .FromInstance(_stateMachine);

        Container.Bind<ITankAI>()
            .To<RandomMovementAI>()
            .AsTransient();

        Container.Bind<RotateState>().AsTransient();
        Container.Bind<MoveState>().AsTransient();
    }
}