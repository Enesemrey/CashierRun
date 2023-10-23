using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePoolable : StandardPoolable
{
    public ParticleSystem particle;

    private void Awake()
    {
        particle = GetComponentInChildren<ParticleSystem>();
        ParticleSystem.MainModule mainModule = particle.main;
        mainModule.stopAction = ParticleSystemStopAction.Callback;
        mainModule.playOnAwake = false;

        PoolLeaveEvent += OnPoolLeave;
    }

    private void OnPoolLeave(StandardPoolable _)
    {
        particle.Play(true);
    }

    private void OnParticleSystemStopped()
    {
        Dispose();
    }
}
