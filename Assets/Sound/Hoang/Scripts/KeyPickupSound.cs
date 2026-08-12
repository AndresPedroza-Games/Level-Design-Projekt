using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class KeyPickupSound : MonoBehaviour
{
    [Header("--- Pickup Sounds ---")]
    [SerializeField] private AudioClip[] pickupSounds;

    [Header("--- Audio Settings ---")]
    [SerializeField][Range(0f, 1f)] private float volume = 1f;

    [SerializeField] private bool randomizePitch = false;

    [SerializeField]
    [Range(0.8f, 1.2f)]
    private float minPitch = 0.95f;

    [SerializeField]
    [Range(0.8f, 1.2f)]
    private float maxPitch = 1.05f;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    public void PlayPickupSound()
    {
        if (pickupSounds == null || pickupSounds.Length == 0)
            return;

        int randomIndex = Random.Range(0, pickupSounds.Length);

        AudioClip clip = pickupSounds[randomIndex];

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