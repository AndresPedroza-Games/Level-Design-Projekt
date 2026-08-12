using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GhostSound : MonoBehaviour
{
    [Header("--- Ghost Sounds ---")]
    [SerializeField] private AudioClip[] ghostSounds;

    [Header("--- Audio Settings ---")]
    [SerializeField][Range(0f, 1f)] private float volume = 1f;

    [SerializeField] private bool randomizePitch = false;

    [SerializeField]
    [Range(0.8f, 1.2f)]
    private float minPitch = 0.95f;

    [SerializeField]
    [Range(0.8f, 1.2f)]
    private float maxPitch = 1.05f;

    [Header("--- 3D Sound ---")]
    [SerializeField] private bool use3DSound = true;

    [SerializeField] private float minDistance = 2f;

    [SerializeField] private float maxDistance = 15f;

    private AudioSource audioSource;

    private int currentSoundIndex = 0;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.loop = false;
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
        PlayNextGhostSound();
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayNextGhostSound();
        }
    }

    private void PlayNextGhostSound()
    {
        if (ghostSounds == null || ghostSounds.Length == 0)
            return;

        AudioClip clip = ghostSounds[currentSoundIndex];

        if (clip != null)
        {
            if (randomizePitch)
            {
                audioSource.pitch = Random.Range(minPitch, maxPitch);
            }
            else
            {
                audioSource.pitch = 1f;
            }

            audioSource.clip = clip;
            audioSource.Play();
        }

        currentSoundIndex++;

        if (currentSoundIndex >= ghostSounds.Length)
        {
            currentSoundIndex = 0;
        }
    }
}