using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class VFX : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> _BoltParticles = new List<ParticleSystem>();

    [SerializeField] private float _CoolDownLightingBolt = 0.1f;

    private bool _CanDropBolt;

    private void Awake()
    {
        _CanDropBolt = true;
    }

    private void Update()
    {
        if (_CanDropBolt)
        {
            _CanDropBolt = false;
            StartCoroutine(RestartBolt());
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
}
