using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class VFX : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> _BoltParticles = new List<ParticleSystem>();
    [SerializeField] private GameObject _Light;

    [SerializeField] private float _CoolDownLightingBolt = 0.1f;
    [SerializeField] private float _CoolDownLight = 0.05f;

    private bool _CanDropBolt;
    private bool _LightIsActive;

    private void Awake()
    {
        _CanDropBolt = true;
        _LightIsActive = true;
    }

    private void Update()
    {
        if (_CanDropBolt)
        {
            _CanDropBolt = false;
            StartCoroutine(RestartBolt());
        }

        if (_LightIsActive)
        {
            _Light.SetActive(_LightIsActive);
            _LightIsActive = false;
            StartCoroutine(StopLight());
        }
    }

    private void StartParticle(List<ParticleSystem> particles)
    {
        foreach (ParticleSystem bolt in particles)
        {
            bolt.Play();
        }
    }
    private void StopParticle(List<ParticleSystem> particles)
    {
        foreach (ParticleSystem bolt in particles)
        {
            bolt.Stop();
        }
    }

    private IEnumerator RestartBolt()
    {
        yield return new WaitForSeconds(_CoolDownLightingBolt);
        StartParticle(_BoltParticles);
        _CanDropBolt = true;
    }

    private IEnumerator StopLight()
    {
        yield return new WaitForSeconds(_CoolDownLight);
        _Light.SetActive(_LightIsActive);
        _LightIsActive = true;
    }
}
