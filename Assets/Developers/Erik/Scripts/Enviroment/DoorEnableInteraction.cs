using UnityEngine;


[RequireComponent(typeof(Collider))]
public class DoorEnableInteraction : MonoBehaviour {

	[Header("---Doors to enable---")]
	[SerializeField] private Door door;

	private Collider col;


	private void Awake() {
		col = GetComponent<Collider>();
		col.isTrigger = true;

		door._DontNeedKey = false;
	}


	private void OnTriggerEnter(Collider other) {
		if (!other.CompareTag("Player"))
			return;

		door._DontNeedKey = true;
	}


	private void OnTriggerExit(Collider other) {
		if (!other.CompareTag("Player"))
			return;

		door._DontNeedKey = false;
	}

}