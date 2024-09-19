using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public Vector3 position;
    public Quaternion rotation;

    public PlayerData()
    {
        position = Vector3.zero;
        rotation = Quaternion.identity;
    }
}