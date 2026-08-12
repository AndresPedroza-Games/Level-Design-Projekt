using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BreathingSound : MonoBehaviour
{
    [Header("--- Breathing Sound ---")]
    [SerializeField] private AudioClip breathingAudio;

    [Header("--- Timing ---")]
    [SerializeField] private float interval = 300f;

    [Header("--- Audio Settings ---")]
    [SerializeField][Range(0f, 1f)] private float volume = 1f;

    private AudioSource audioSource;
    private float timer;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.volume = volume;
    }

    private void Start()
    {
        timer = interval;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            PlayBreathingSound();

            timer = interval;
        }
    }

    private void PlayBreathingSound()
    {
        if (breathingAudio == null)
            return;

        audioSource.PlayOneShot(breathingAudio, volume);
    }
}