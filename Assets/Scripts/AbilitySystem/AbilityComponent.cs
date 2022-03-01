using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JetBrains.Annotations;
using Unity.Jobs;
using UnityEngine;

public class AbilityComponent : MonoBehaviour
{
    private List<AbilityItem> _abilities = new List<AbilityItem>();
    public Ability[] Abilities => _abilities.Select(x=>x.value).ToArray();

    private EntityComponent _entityComponent;
    public EntityComponent EntityComponent
    {
        get
        {
            _entityComponent ??= GetComponent<EntityComponent>();
            return _entityComponent;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //recount remaining time
        for (int i = 0; i < _abilities.Count; i++)
        {
            var ability = _abilities[i];
            if (float.IsPositiveInfinity(ability.value.Duration)) //if ability is infinite dont process it
                continue;
            
            ability.value.DurationRemaining -= Time.deltaTime;
            if (ability.value.DurationRemaining <= 0) 
                _abilities.Remove(ability);
        }
        
        //ensure every job is completed
        for (int i = 0; i < _abilities.Count; i++)
        {
            var abilityItem = _abilities[i];
            abilityItem.jobHandle?.Complete();
        }
        
        //this one is aimed at creating jobs
        for (int i = 0; i < _abilities.Count; i++)
        {
            var abilityItem = _abilities[i];
            abilityItem.jobHandle = abilityItem.value.OnUpdateScheduleJob(abilityItem.jobHandle);
        }
        
        //this one is aimed at common things
        for (int i = 0; i < _abilities.Count; i++)
        {
            var abilityItem = _abilities[i];
            abilityItem.value.OnUpdate(abilityItem.jobHandle);
        }
    }

    private void LateUpdate()
    {
        //this one is aimed at common things
        for (int i = 0; i < _abilities.Count; i++)
        {
            var abilityItem = _abilities[i];
            abilityItem.value.OnLateUpdate(abilityItem.jobHandle);
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < _abilities.Count; i++)
        {
            _abilities[i].value.OnDestroy();
        }
    }

    public void Add(Ability ability)
    {
        ability.transform = this.transform;
        ability.entityComponent = GetComponent<EntityComponent>();
        _abilities.Add(new AbilityItem() with{value = ability, jobHandle = null});
        ability.OnStart();
    }
    
    public T AddOrGet<T>() where T : Ability
    {
        if(_abilities.Any(x=>x.GetType() == typeof(T)))
        {
            return (T)_abilities.First(x => x.GetType() == typeof(T)).value;
        }
        var ability = Activator.CreateInstance<T>();
        ability.transform = this.transform;
        ability.entityComponent = GetComponent<EntityComponent>();
        _abilities.Add(new AbilityItem {value = ability, jobHandle = null});
        ability.OnStart();
        return ability;
    }
    
    [CanBeNull]
    public T Get<T>() where T : Ability
    {
        return (T)_abilities.FirstOrDefault(x => x.GetType() == typeof(T))?.value;
    }

    public void Remove(Ability ability)
    {
        _abilities.Remove(_abilities.FirstOrDefault(x=>x.value == ability));
        ability.OnDestroy();
    }

    public void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < _abilities.Count; i++)
        {
            var abilityItem = _abilities[i];
            abilityItem.value.OnTriggerEnter(other);
        }
    }
    
    public void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < _abilities.Count; i++)
        {
            var abilityItem = _abilities[i];
            abilityItem.value.OnTriggerExit(other);
        }
    }
}

public record AbilityItem
{
    public Ability value;
    public JobHandle? jobHandle;
}