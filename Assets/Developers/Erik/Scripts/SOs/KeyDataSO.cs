using UnityEngine;

[CreateAssetMenu(fileName = "New Key", menuName = "Items/Key")]
public class KeyDataSO : ScriptableObject
{
    public string Name;
    public int id;
    public GameObject keyPrefab;
}
