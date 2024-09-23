using UnityEngine;


public class AvailableAreaDetector : MonoBehaviour
{
    [SerializeField] private float _areaRadius;
    [SerializeField] private LayerMask _detectionLayer;

    public bool IsAreaAvailable(Vector2 areaCenter, LayerMask detectionLayer)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(areaCenter, _areaRadius, detectionLayer);
        return colliders.Length == 0;
    }
}
