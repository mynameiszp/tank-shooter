using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private Rigidbody2D _rigidbody;

    public void Move()
    {
        _rigidbody.AddForce(transform.right * _bulletSpeed, ForceMode2D.Impulse);
    }
}
