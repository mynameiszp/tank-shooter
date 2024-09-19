using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class FixedPointsSpawnStrategy : ISpawnStrategy
{
    private Vector2[] _fixedPoints;

    [Inject]
    public FixedPointsSpawnStrategy(Vector2[] fixedPoints)
    {
        _fixedPoints = fixedPoints;
    }

    public List<Vector2> GetSpawnPoints()
    {
        return new List<Vector2>(_fixedPoints);
    }
}
