using UnityEngine;

public class BasicTankMovement : IMovementStrategy
{
    private float _movementSpeed;
    private float _rotationDegree;

    public BasicTankMovement(float movementSpeed, float rotationDegree)
    { 
        _movementSpeed = movementSpeed;
        _rotationDegree = rotationDegree;
    }

    public Vector3 GetDirection(Vector2 input)
    {
        return new Vector2(input.y * _movementSpeed, 0);
    }

    public Vector3 GetRotation(Vector2 input)
    {
        return new Vector3(0f, 0f, -input.x * _rotationDegree);
    }
}
