using System;
using UnityEngine;


public class SoundManager : MonoBehaviour {

	public static SoundManager Instance;

	public Sound[] sounds;


	private void Awake() {
		if (Instance != null && Instance != this) {
			Destroy(gameObject);
			return;
		}
		else {
			Instance = this;
		}

		SetupAudioSources();
	}


	private void SetupAudioSources() {
		foreach (Sound s in sounds) {
			s.source = gameObject.AddComponent<AudioSource>();

			s.source.clip = s.clip;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.playOnAwake = s.playOnAwake;
			s.source.loop = s.loop;
		}
	}


	public void PlaySound(string name) {
		Sound s = Array.Find(sounds, sound => sound.name == name);

		s?.source.Play();
	}

}