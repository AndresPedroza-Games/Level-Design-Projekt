using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour {
    public Key CurrentKey { get; private set; }

    public Transform keySocket;
    public Transform itemDropPos;


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
            PickUpKey(key.gameObject);
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
        PickUpKey(newKey.gameObject);
    }



    private void PickUpKey(GameObject key) {
        key.transform.SetParent(keySocket);
        key.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }



    private void DropKey(GameObject key) {
        if (CurrentKey == null)
            return;

        CurrentKey = null;

        if (key.TryGetComponent(out Key keyComp))
            keyComp.EnablePhysics();

        key.transform.parent = null;
        key.transform.position = itemDropPos.position;
    }


    private void DropKeyAction(InputAction.CallbackContext ctx) {
        if (CurrentKey != null)
            DropKey(CurrentKey.gameObject);
    }



    private void OnDrawGizmosSelected() {
        if (itemDropPos != null) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(itemDropPos.position, 0.2f);
        }
    }
}
