using UnityEngine;

public class AvailableAreaDetector : MonoBehaviour
{
    [SerializeField] private float _areaRadius;
    [SerializeField] private LayerMask _detectionLayer;

    public bool IsAreaAvailable(Vector2 areaCenter)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(areaCenter, _areaRadius, _detectionLayer);
        return colliders.Length == 0;
    }
}
