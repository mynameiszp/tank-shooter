using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyTank : Tank
{
    public event Action<GameObject> OnDestroy;
    public event Action OnCollision;
    public event Action<GameObject> OnEnemiesCollision;

    private int _boundaryLayer;
    private int _playerLayer;
    private int _bulletLayer;
    private int _enemyLayer;

    private const string PLAYER_LAYER = GameConstants.PLAYER_LAYER_NAME;
    private const string BOUNDARY_LAYER = GameConstants.BOUNDARY_LAYER_NAME;
    private const string BULLET_LAYER = GameConstants.BULLET_LAYER_NAME;
    private const string ENEMY_LAYER = GameConstants.ENEMY_LAYER_NAME;

    private void Start()
    {
        InitializeLayers();
    }

    private void InitializeLayers()
    {
        _playerLayer = LayerMask.NameToLayer(PLAYER_LAYER);
        _boundaryLayer = LayerMask.NameToLayer(BOUNDARY_LAYER);
        _bulletLayer = LayerMask.NameToLayer(BULLET_LAYER);
        _enemyLayer = LayerMask.NameToLayer(ENEMY_LAYER);
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
        else if (collisionLayer == _enemyLayer)
        {
            if (transform.position.y < collision.transform.position.y)
            {
                HandleCollisionWithOtherEnemy(collision.gameObject);
            }
        }
    }

    private void HandleCollisionWithOtherEnemy(GameObject otherEnemy)
    {
        int decision = Random.Range(0, 2);
        if (decision == 0)
        {
            OnEnemiesCollision?.Invoke(gameObject);
        }
        else
        {
            OnEnemiesCollision?.Invoke(otherEnemy.gameObject);
        }
    }
}
