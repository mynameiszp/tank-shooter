using UnityEngine;

public abstract class Tank : MonoBehaviour
{
    [SerializeField] protected Transform barrel;
    private Quaternion _relativeRotation = Quaternion.identity;
    private Vector3 _relativePosition;

    public void SetRelativePosition()
    {
        _relativePosition = GetRightEdgePosition(barrel);
    }
    public abstract void Fire();

    public void InitializeBullet(Bullet bullet)
    {
        Vector3 absolutePosition = transform.TransformPoint(_relativePosition);
        Quaternion absoluteRotation = transform.rotation * _relativeRotation;
        bullet.InitializeBullet(absolutePosition, absoluteRotation);
    }

    protected Vector3 GetRightEdgePosition(Transform transform)
    {
        if (!transform.TryGetComponent(out Collider2D collider))
        {
            return transform.position;
        }
        Vector3 localRightEdge = new Vector3(collider.bounds.extents.x, 0, 0);
        Vector3 worldRightEdge = transform.TransformPoint(localRightEdge + collider.bounds.center);
        return worldRightEdge;
    }
}
