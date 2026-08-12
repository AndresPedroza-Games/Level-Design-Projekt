using UnityEngine;

public class Cinematic : MonoBehaviour
{
    [SerializeField] private GameObject _FlashLight;

    private Lighting _Lighting;

    private void Awake()
    {
        _Lighting = GetComponent<Lighting>();
    }

    public void CameraFall()
    {
        _FlashLight.SetActive(true);
        _Lighting.enabled = true;
    }

    public void CameraDisapear()
    {
        _FlashLight.SetActive(false);
        _Lighting.enabled = false;
    }
}
