using UnityEngine;

public class Box : MonoBehaviour, IInteractable, IPullable
{
    public void Interact(Player player)
    {
        Debug.Log("Hola");
    }

    public void Pull()
    {
        
    }
}
