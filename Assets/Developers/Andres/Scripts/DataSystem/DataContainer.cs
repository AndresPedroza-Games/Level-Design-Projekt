using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DataContainer", menuName = "Data Manager/DataContainer")]
public class DataContainer : ScriptableObject
{
    public List<Key> keyData;
    public List<Door> doorData;
}
