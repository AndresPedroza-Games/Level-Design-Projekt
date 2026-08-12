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
        if (!characterController.isGrounded)
        {
            distanceSinceLastStep = 0f;
            return;
        }

        if (playerController.MoveInput.magnitude <= 0.01f)
        {
            distanceSinceLastStep = 0f;
            return;
        }

        Vector3 velocity = characterController.velocity;
        velocity.y = 0f;

        float currentSpeed = velocity.magnitude;

        if (currentSpeed <= 0.01f)
            return;

        distanceSinceLastStep += currentSpeed * Time.deltaTime;

        float requiredDistance = playerController.IsRunning
            ? runStepDistance
            : walkStepDistance;

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

        lastFootstepIndex = randomIndex;

        distanceSinceLastStep = 0f;
    }
}