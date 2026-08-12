using System;
using UnityEngine;
using UnityEngine.SceneManagement;


[DefaultExecutionOrder(-10)]
public class EventSystemController : MonoBehaviour
{
    public static EventSystemController eventSystemController;

    public Action<Door> onOpenDoor;
    public Action onRestart;

    public Action onEndGame;

    public Action onInteractWithLock;
    public Action onEndInteractionWithLock;
    public Action onPuzzleCompleted;

    public Action onReleasePiece;
    public Action<Vector2> onRotateLock;

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

    public void EndGame()
    {
        onEndGame?.Invoke();
    }


    public void LoadMainMenu() {
	    SceneManager.LoadScene(0);
    }

    //Lock Events//

    public void InteractWithLock()
    {
        onInteractWithLock?.Invoke();
    }

    public void ExitLock()
    {
        onEndInteractionWithLock?.Invoke();
    }

    public void RotateLock(Vector2 direction)
    {
        onRotateLock?.Invoke(direction);
    }

    public void ReleasePiece()
    {
        onReleasePiece?.Invoke();
    }

    public void PuzzleCompleted()
    {
        onPuzzleCompleted?.Invoke();
    }

}
