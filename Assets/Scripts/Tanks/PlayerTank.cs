using System;
using UnityEngine;
using Zenject;

public class PlayerTank : Tank
{
    public event Action OnDestroy;

    [Inject] private ObjectPool _objectPool;
    private int _enemyLayer;
    private const string ENEMY_LAYER = GameConstants.ENEMY_LAYER_NAME;

    private void Start()
    {
        SetRelativePosition();
        _enemyLayer = LayerMask.NameToLayer(ENEMY_LAYER);
    }

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int collisionLayer = collision.gameObject.layer;
        if (collisionLayer == _enemyLayer)
        {
            OnDestroy?.Invoke();
        }
    }
}
