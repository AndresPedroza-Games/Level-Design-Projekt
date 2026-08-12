using System;
using UnityEngine;

[Serializable]
public class HoldProfiler
{

    [Header("---Config---")]
    public bool keepMomentum;
    public bool followRotation;

    [Header("---RigidBody---")]
    public bool useGravity;
    public RigidbodyConstraints constraints;

    [Header("---Joint---")]
    public ConfigurableJointMotion xMotion;
    public ConfigurableJointMotion yMotion;
    public ConfigurableJointMotion zMotion;

    public ConfigurableJointMotion angularXMotion;
    public ConfigurableJointMotion angularYMotion;
    public ConfigurableJointMotion angularZMotion;
}
