using UnityEngine;

public class Player : MonoBehaviour
{
    public Inventory Inventory { get; private set; }

    void Start()
    {
        Inventory = GetComponent<Inventory>();
    }
    
}
