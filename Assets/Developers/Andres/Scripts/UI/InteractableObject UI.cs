using TMPro;
using UnityEngine;

public class InteractableObjectUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _TextBox;
    [SerializeField] private float _MinDistance;
    [SerializeField] private Transform _Parent;

    private PlayerController _PlayerController;
    private Key _Key;

    private void Awake()
    {
        _PlayerController = FindAnyObjectByType<PlayerController>();
        _Key = GetComponentInParent<Key>();
    }

    private void Update()
    {
        if (!_Key.isGrabbed)
            SetActiveTextBox(CheckDistance());
        else
            SetActiveTextBox(false);
    }

    private void SetActiveTextBox(bool status)
    {
        _TextBox.gameObject.SetActive(status);
    }

    private bool CheckDistance()
    {
        Vector3 playerPos = _PlayerController.gameObject.transform.position;
        Vector3 itemPos = _Parent.transform.position;

        float currentDistance = Vector3.Distance(playerPos, itemPos);

        if (currentDistance <= _MinDistance)
            return true;

        return false;
    }
}
