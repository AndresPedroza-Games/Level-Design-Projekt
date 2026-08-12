using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FootstepSystem : MonoBehaviour
{
    [Header("--- Footstep Sounds ---")]
    [SerializeField] private AudioClip[] footstepSounds;

    [Header("--- Footstep Settings ---")]
    [SerializeField] private float walkStepDistance = 1.8f;
    [SerializeField] private float runStepDistance = 2.2f;

    [Header("--- Audio Settings ---")]
    [SerializeField][Range(0f, 1f)] private float volume = 1f;
    [SerializeField] private bool randomizePitch = false;
    [SerializeField][Range(0.8f, 1.2f)] private float minPitch = 0.95f;
    [SerializeField][Range(0.8f, 1.2f)] private float maxPitch = 1.05f;

    private PlayerController playerController;
    private CharacterController characterController;
    private AudioSource audioSource;

    private float distanceSinceLastStep = 0f;

    private int lastFootstepIndex = -1;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        characterController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        HandleFootsteps();
    }

    private void HandleFootsteps()
    {
        // Kein Sound, wenn der Spieler nicht am Boden ist
        if (!characterController.isGrounded)
        {
            distanceSinceLastStep = 0f;
            return;
        }

        // Kein Sound, wenn der Spieler sich nicht bewegt
        if (playerController.MoveInput.magnitude <= 0.01f)
        {
            distanceSinceLastStep = 0f;
            return;
        }

        // Geschwindigkeit des CharacterControllers
        Vector3 velocity = characterController.velocity;
        velocity.y = 0f;

        float currentSpeed = velocity.magnitude;

        // Spieler bewegt sich praktisch nicht
        if (currentSpeed <= 0.01f)
            return;

        // Zurückgelegte Strecke seit dem letzten Fußschritt
        distanceSinceLastStep += currentSpeed * Time.deltaTime;

        // Unterschiedliche Schrittweite beim Laufen und Rennen
        float requiredDistance = playerController.IsRunning
            ? runStepDistance
            : walkStepDistance;

        // Genug Strecke für einen neuen Fußschritt?
        if (distanceSinceLastStep >= requiredDistance)
        {
            PlayNextFootstep();

            distanceSinceLastStep = 0f;
        }
    }

    private void PlayNextFootstep()
    {
        if (footstepSounds == null || footstepSounds.Length == 0)
            return;

        int randomIndex;

        // Verhindert, dass derselbe Sound zweimal direkt hintereinander kommt
        do
        {
            randomIndex = Random.Range(0, footstepSounds.Length);
        }
        while (footstepSounds.Length > 1 && randomIndex == lastFootstepIndex);

        AudioClip clip = footstepSounds[randomIndex];

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

            audioSource.PlayOneShot(clip, volume);
        }

        // Speichert den zuletzt abgespielten Sound
        lastFootstepIndex = randomIndex;

        // Abstand zurücksetzen
        distanceSinceLastStep = 0f;
    }
}