using Zenject;

public class PlayerTank : Tank
{
    [Inject] protected ObjectPool objectPool;

    public override void Fire()
    {
        var bullet = objectPool.GetPlayerBullet();
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
