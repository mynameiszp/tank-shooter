using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolConfig
{
    public Transform parentObject;
    public int poolCapacity;
    public GameObject objectPrefab;
    public List<GameObject> pooledObjects;
}
