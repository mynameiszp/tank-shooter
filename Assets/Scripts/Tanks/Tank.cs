using UnityEngine;
using Zenject;

public abstract class Tank : MonoBehaviour
{
    [Inject] protected ObjectPool objectPool;
    [SerializeField] private Transform _barrel;
    private Quaternion _relativeRotation = Quaternion.identity;
    private Quaternion _absoluteRotation;
    private Vector3 _relativePosition;
    private Vector3 _absolutePosition;

    private void Start()
    {
        _relativePosition = _barrel.position;
    }
    public abstract void Fire();

    public virtual void InitializeBullet(GameObject bullet)
    {
        _absolutePosition = transform.TransformPoint(_relativePosition);
        _absoluteRotation = transform.rotation * _relativeRotation;
        bullet.transform.SetPositionAndRotation(_absolutePosition, _absoluteRotation);
        bullet.SetActive(true);
    }
}
