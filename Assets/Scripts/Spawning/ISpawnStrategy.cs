using System.Collections.Generic;
using UnityEngine;

public interface ISpawnStrategy
{
    List<Vector2> GetSpawnPoints();
}
