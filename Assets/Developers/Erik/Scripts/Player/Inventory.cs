using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour {
    public Key CurrentKey { get; private set; }

    public Transform keySocket;
    [SerializeField] private float dropOffset = 1.5f;


    private void OnEnable() {
        InputManager.Instance.Controls.Interaction.Drop.performed += DropKeyAction;

    }

    private void OnDisable() {
        InputManager.Instance.Controls.Interaction.Drop.performed -= DropKeyAction;
    }



    public void AddKey(Key key) {
        if (CurrentKey != null) {
            SwitchKeys(key);
        }
        else {
            CurrentKey = key;
            TakeKey(key.gameObject);
        }
    }


    public void UseKey() {
        if (CurrentKey != null) {
            CurrentKey.gameObject.SetActive(false);
            CurrentKey = null;
        }
    }


    private void SwitchKeys(Key newKey) {
        DropKey(CurrentKey.gameObject);

        CurrentKey = newKey;
        TakeKey(newKey.gameObject);
    }



    private void TakeKey(GameObject key) {
        key.transform.SetParent(keySocket);
        key.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }



    private void DropKey(GameObject key) {
        if (CurrentKey != null)
            CurrentKey = null;

        key.transform.parent = null;
        key.transform.position = transform.position + transform.forward * dropOffset;
        if (key.TryGetComponent(out Key keyComp))
            keyComp.EnablePhysics();
    }


    private void DropKeyAction(InputAction.CallbackContext ctx) {
        if (CurrentKey != null)
            DropKey(CurrentKey.gameObject);

    }



    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Vector3 center = transform.position + transform.forward * dropOffset;
        Gizmos.DrawWireSphere(center, 0.2f);
    }
}
