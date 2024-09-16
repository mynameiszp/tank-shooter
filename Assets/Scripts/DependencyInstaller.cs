using UnityEngine;
using Zenject;

public class DependencyInstaller : MonoInstaller
{
    [SerializeField] private ObjectPool _objectPool;

    public override void InstallBindings()
    {
        Container.Bind<ObjectPool>()
            .To<ObjectPool>()
            .FromInstance(_objectPool);
    }
}