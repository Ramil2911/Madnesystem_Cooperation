using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Jobs;

public abstract class Ability
{
    public abstract float DurationRemaining { get; set; }
    public abstract float Duration { get; set; }
    public abstract string Name { get; set; }
    public abstract string Description { get; set; }

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
    
    public static Type GetById(int id)
    {
        return Assembly.GetExecutingAssembly().GetTypes()
            .FirstOrDefault(t => t.IsClass && !t.IsAbstract && t.HasAttribute<AbilityId>() &&
                                 t.GetCustomAttribute<AbilityId>().id == id);
    }
}
