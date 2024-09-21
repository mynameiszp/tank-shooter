using UnityEngine;

public abstract class Tank : MonoBehaviour
{
    [SerializeField] private Transform _barrel;
    private Quaternion _relativeRotation = Quaternion.identity;
    private Vector3 _relativePosition;

    public void SetRelativePosition()
    {
        _relativePosition = GetRightEdgePosition(_barrel);
    }
    public abstract void Fire();

    public virtual void InitializeBullet(GameObject bullet)
    {
        Vector3 absolutePosition = transform.TransformPoint(_relativePosition);
        Quaternion absoluteRotation = transform.rotation * _relativeRotation;
        bullet.transform.SetPositionAndRotation(absolutePosition, absoluteRotation);
        bullet.SetActive(true);
    }

    private Vector3 GetRightEdgePosition(Transform transform)
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
