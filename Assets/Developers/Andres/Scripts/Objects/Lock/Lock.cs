using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.ProBuilder.Shapes;

public class Lock : MonoBehaviour
{
    public static Lock Instance;

    [SerializeField] private List<int> _Password = new List<int>();
    [SerializeField] private List<LockPiece> _LockPiecesList;
    [SerializeField] private Door _Door;

    private EventSystemController _EventSystemController;

    private List<int> _CurrentCombination = new List<int>();
    private Dictionary<int, int> _AngleToPassword = new Dictionary<int, int>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        _EventSystemController = EventSystemController.eventSystemController;
        _EventSystemController.onReleasePiece += PuzzleCompleted;

        _AngleToPassword = new()
        {
            {0,6},
            {36,5},
            {72,4},
            {108,3},
            {144,2},
            {180,1},
            {-144, 0},
            {-108,9},
            {-72,8},
            {-36,7}
        };
    }

    private void PuzzleCompleted()
    {
        if (CheckIfPuzzleCompleted())
        {
            _EventSystemController.PuzzleCompleted();
            _Door.OpenDoor();
            Debug.Log("Puzzle Completed");
        }
    }

    private bool CheckIfPuzzleCompleted()
    {
        _CurrentCombination.Clear();

        foreach (LockPiece piece in _LockPiecesList)
        {
            float angle = piece._Steps * 36f;
            int currentAngle = Mathf.DeltaAngle(0f, angle).ConvertTo<int>();
            int number = _AngleToPassword[currentAngle];
            _CurrentCombination.Add(number);
        }

        for (int number = 0; number < _Password.Count; number++)
        {
            if (_CurrentCombination[number] != _Password[number])
                return false;
        }

        return true;
    }
}
