using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class KeyImpactSound : MonoBehaviour
{
    [Header("--- Impact Sounds ---")]
    [SerializeField] private AudioClip[] impactSounds;

    [Header("--- Audio Settings ---")]
    [SerializeField][Range(0f, 1f)] private float volume = 1f;
    [SerializeField] private bool randomizePitch = false;
    [SerializeField][Range(0.8f, 1.2f)] private float minPitch = 0.95f;
    [SerializeField][Range(0.8f, 1.2f)] private float maxPitch = 1.05f;

    [Header("--- Impact Settings ---")]
    [SerializeField] private float minimumImpactVelocity = 1f;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        // Audio Source Einstellungen
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 1f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        float impactVelocity = collision.relativeVelocity.magnitude;

        if (impactVelocity < minimumImpactVelocity)
            return;

        PlayRandomImpactSound();
    }

    private void PlayRandomImpactSound()
    {
        if (impactSounds == null || impactSounds.Length == 0)
            return;

        int randomIndex = Random.Range(0, impactSounds.Length);

        AudioClip clip = impactSounds[randomIndex];

        if (clip == null)
            return;

        if (randomizePitch)
        {
            audioSource.pitch = Random.Range(minPitch, maxPitch);
        }
        else
        {
            audioSource.pitch = 1f;
        }

        audioSource.PlayOneShot(clip, volume);
    }
}