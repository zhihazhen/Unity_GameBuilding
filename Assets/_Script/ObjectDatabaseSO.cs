using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ObjectDatabaseSO : ScriptableObject
{
    /*// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
    public List<ObjectData> objectsData;
}
[Serializable]
public class ObjectData
{
    [field: SerializeField]
    public string Name
    {
        get;
        private set;
    }

    [field: SerializeField]
    public int ID
    {
        get;
        private set;
    }

    [field: SerializeField] public Vector2Int Size { get; private set; } = Vector2Int.one;
    [field: SerializeField]
    public GameObject Prefab { get; private set; }

}
