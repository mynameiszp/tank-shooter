using System;
using UnityEngine;

public class EnemyTank : Tank
{
    public event Action<GameObject> OnDestroy;
    public event Action OnCollision;

    private int _boundaryLayer;
    private int _playerLayer;
    private int _bulletLayer;

    private const string PLAYER_LAYER = GameConstants.PLAYER_LAYER_NAME;
    private const string BOUNDARY_LAYER = GameConstants.BOUNDARY_LAYER_NAME;
    private const string BULLET_LAYER = GameConstants.BULLET_LAYER_NAME;

    private void Start()
    {
        InitializeLayers();
    }

    private void InitializeLayers()
    {
        _playerLayer = LayerMask.NameToLayer(PLAYER_LAYER);
        _boundaryLayer = LayerMask.NameToLayer(BOUNDARY_LAYER);
        _bulletLayer = LayerMask.NameToLayer(BULLET_LAYER);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int collisionLayer = collision.gameObject.layer;
        if (collisionLayer == _playerLayer || collisionLayer == _boundaryLayer)
        {
            OnCollision?.Invoke();
        }
        else if (collisionLayer == _bulletLayer)
        {
            OnDestroy?.Invoke(gameObject);
        }
    }
}
