using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class FootstepSoundPlayer : MonoBehaviour {

	[Header("---Interaction Layer---")]
	[SerializeField] private LayerMask interactionLayer;

	private AudioSource audioSource;


	private void Awake() {
		audioSource = GetComponent<AudioSource>();
	}


	private void OnTriggerEnter(Collider other) {
		if (audioSource.isPlaying) return;
		
		if ((interactionLayer.value & (1 << other.gameObject.layer)) != 0) {
			audioSource.Play();
		}
	}

}