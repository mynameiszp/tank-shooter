using UnityEngine;

public interface ITankAI
{
    Vector3 GetMovementDirection();   
    Quaternion GetRotation(Vector3 rotation);         
}
