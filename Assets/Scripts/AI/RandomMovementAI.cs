using UnityEngine;

public class RandomMovementAI : ITankAI
{
    public Vector3 GetMovementDirection()
    {
        return Vector3.right;
    }

    public Quaternion GetRotation(Vector3 rotation)
    {
        return Quaternion.Euler(0, 0, GetRandomSign() * rotation.z);
    }

    private int GetRandomSign()
    {
        return Random.value > 0.5f ? 1 : -1;
    }
}
