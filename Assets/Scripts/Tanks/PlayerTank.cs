using Zenject;

public class PlayerTank : Tank
{
    [Inject] private ObjectPool _objectPool;

    public override void Fire()
    {
        var bullet = _objectPool.GetPlayerBullet();
        if (bullet != null)
        {
            InitializeBullet(bullet);
            if (bullet.TryGetComponent(out PlayerBullet bulletComponent))
            {
                bulletComponent.Move();
            }
        }
    }
}
