using UnityEngine;

public class PlayerBullet : Bullet
{
    private int _boundaryLayer;
    private int _enemyLayer;

    void Start()
    {
        _boundaryLayer = LayerMask.NameToLayer(GameConstants.BOUNDARY_LAYER_NAME);
        _enemyLayer = LayerMask.NameToLayer(GameConstants.ENEMY_LAYER_NAME);
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
