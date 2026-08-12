using UnityEngine;


public class EndScreen : MonoBehaviour {

	[Header("---End Screen---")]
	[SerializeField] private GameObject endScreen;


	private void Awake() {
		endScreen.SetActive(false);
	}


	public void EnableEndScreen() {
		endScreen.SetActive(true);
		Time.timeScale = 0f;

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		InputManager.Instance.Controls.Disable();
		Player.Instance.FreezeCharacter(true);
	}

}