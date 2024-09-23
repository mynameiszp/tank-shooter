using UnityEngine;

[System.Serializable]
public class EnemyData
{
    public Vector3 position;
    public Quaternion rotation;

    public EnemyData()
    {
        position = Vector3.zero;
        rotation = Quaternion.identity;
    }
}