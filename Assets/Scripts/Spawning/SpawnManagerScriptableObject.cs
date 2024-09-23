using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnManagerScriptableObject")]
public class SpawnManagerScriptableObject : ScriptableObject
{
    public Vector2[] spawnPoints;
}
