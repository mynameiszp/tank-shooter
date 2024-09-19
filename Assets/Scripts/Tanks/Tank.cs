using UnityEngine;

public abstract class Tank : MonoBehaviour
{
    [SerializeField] private Transform _barrel;
    private Quaternion _relativeRotation = Quaternion.identity;
    private Vector3 _relativePosition;

    private void Start()
    {
        _relativePosition = _barrel.position;
    }
    public abstract void Fire();

    public virtual void InitializeBullet(GameObject bullet)
    {
        Vector3 absolutePosition = transform.TransformPoint(_relativePosition);
        Quaternion absoluteRotation = transform.rotation * _relativeRotation;
        bullet.transform.SetPositionAndRotation(absolutePosition, absoluteRotation);
        bullet.SetActive(true);
    }
}
