using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ClockSound : MonoBehaviour
{
    [Header("--- Clock Sound ---")]
    [SerializeField] private AudioClip clockAudio;

    [Header("--- Audio Settings ---")]
    [SerializeField][Range(0f, 1f)] private float volume = 1f;

    [Header("--- 3D Sound ---")]
    [SerializeField] private bool use3DSound = true;
    [SerializeField] private float minDistance = 2f;
    [SerializeField] private float maxDistance = 10f;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.loop = true;
        audioSource.volume = volume;

        if (use3DSound)
        {
            audioSource.spatialBlend = 1f;
            audioSource.minDistance = minDistance;
            audioSource.maxDistance = maxDistance;
        }
        else
        {
            audioSource.spatialBlend = 0f;
        }
    }

    private void Start()
    {
        StartClockSound();
    }

    private void Update()
    {
        // Falls der Sound aus irgendeinem Grund aufhört,
        // wird er automatisch wieder gestartet.
        if (!audioSource.isPlaying)
        {
            StartClockSound();
        }
    }

    private void StartClockSound()
    {
        if (clockAudio == null)
        {
            Debug.LogWarning("ClockSound: Keine Audio-Datei zugewiesen!", this);
            return;
        }

        audioSource.clip = clockAudio;
        audioSource.loop = true;

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}