using UnityEngine;
using System.Collections;

public class Lighting : MonoBehaviour
{
    [SerializeField] private GameObject _Light;

    [SerializeField] private float _CoolDownLight = 0.05f;

    private bool _LightIsActive;

    private void Awake()
    {
        _LightIsActive = true;
    }

    private void Update()
    {
        if (_LightIsActive)
        {
            _Light.SetActive(true);
            _LightIsActive = false;
            StartCoroutine(StopLight());
        }
    }

    private IEnumerator StopLight()
    {
        yield return new WaitForSeconds(_CoolDownLight);
        _Light.SetActive(false);
        _LightIsActive = true;
    }
}
