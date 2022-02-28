using System;
using JetBrains.Annotations;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

[Serializable]
public abstract class Ability
{
    public abstract float DurationRemaining { get; set; }
    public abstract float Duration { get; set; }

    public Transform transform;
    public EntityComponent entityComponent;

    private TransformAccessArray _transformAccess;


    public virtual void OnStart()
    {
        
    }
    
    [CanBeNull]
    public virtual JobHandle? OnUpdateScheduleJob([CanBeNull] JobHandle? handle)
    {
        return null;
    }

    public virtual void OnUpdate([CanBeNull] JobHandle? handle)
    {
        
    }

    public virtual void OnLateUpdate([CanBeNull] JobHandle? handle)
    {
        
    }

    public virtual void OnDestroy()
    {
        
    }
    
    public virtual void OnTriggerEnter(Collider other)
    {
        
    }

    public virtual void OnTriggerExit(Collider other)
    {
        
    }
}
