using UnityEngine;

[CreateAssetMenu(fileName = "TankConfig", menuName = "ScriptableObjects/TankConfig")]
public class TankConfig : ScriptableObject
{
    public float moveSpeed;
    public float rotationSpeed;
}
