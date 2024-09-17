public class PlayerTank : Tank
{
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
