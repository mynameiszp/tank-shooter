using UnityEngine;

public interface IMovementStrategy
{
    Vector3 GetDirection(Vector2 input);
    Vector3 GetRotation(Vector2 input);
}
