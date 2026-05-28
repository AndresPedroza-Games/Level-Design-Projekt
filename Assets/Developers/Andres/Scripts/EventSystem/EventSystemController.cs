using System;
using UnityEngine;

public class EventSystemController : MonoBehaviour
{
    public static EventSystemController eventSystemController;

    public Action<Door> onOpenDoor;
    public Action onRestart;

    private void Awake()
    {
        if (eventSystemController == null)
            eventSystemController = this;
    }

    public void OpenDoor(Door door)
    {
        if (onOpenDoor != null)
            onOpenDoor.Invoke(door);
    }

    public void Restart()
    {
        if (onRestart != null)
            onRestart.Invoke();
    }
}
