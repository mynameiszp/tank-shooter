using UnityEngine;

public class PlayerBullet : Bullet
{
    private int _enemyLayer;
    private int _boundaryLayer;

    private const string ENEMY_LAYER = GameConstants.ENEMY_LAYER_NAME;
    private const string BOUNDARY_LAYER = GameConstants.BOUNDARY_LAYER_NAME;

    void Start()
    {
        _boundaryLayer = LayerMask.NameToLayer(BOUNDARY_LAYER);
        _enemyLayer = LayerMask.NameToLayer(ENEMY_LAYER);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int collisionLayer = collision.gameObject.layer;

        switch (collisionLayer)
        {
            case int layer when layer == _boundaryLayer:
                gameObject.SetActive(false);
                break;

            case int layer when layer == _enemyLayer:
                gameObject.SetActive(false);
                collision.gameObject.SetActive(false);
                break;
        }
    }
}
