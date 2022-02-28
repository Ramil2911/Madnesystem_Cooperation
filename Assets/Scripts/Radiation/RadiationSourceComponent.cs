using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Obsolete]
public class RadiationSourceComponent : MonoBehaviour
{
#if UNITY_EDITOR
    [Header("Readonly debug properties")]
    public int recordsAmount;

    public void ProcessDebugSymbols()
    {
        recordsAmount = _records.Count;
    }
#endif
    [Header("Main properties")]
    public float radius = 0;
    public float damage = 0;
    public float fadeDuration = 0; //tbd
    
    public float inheritedRadius = 0;
    public float inheritedDamage = 0;
    public float inheritedDuration = 0;//tbd

    public EntityComponent entityComponent;

    public GameObject particles;

    private List<RadiationSystem.RadiationSourceData> _records = new List<RadiationSystem.RadiationSourceData>();

    private RadiationSystem _radiationSystem;
    
    private SphereCollider _sphereCollider;
    private GameObject _particleSource;
    private MeshCollider _uranCollider;

    public bool isPlayer = false;
    private void Start()
    {
        _radiationSystem = GameObject.FindWithTag("RadiationSystem").GetComponent<RadiationSystem>();
        
        if(gameObject.CompareTag("Player"))
        {
            isPlayer = true;
        }
        TryGetComponent(out entityComponent);
        _sphereCollider = this.gameObject.AddComponent<SphereCollider>();
        _sphereCollider.radius = radius;
        _sphereCollider.isTrigger = true;

        var transform1 = this.transform;
        if(isPlayer == false)
        {
            _particleSource = Instantiate(particles, transform1.position, Quaternion.identity, transform1);
            _uranCollider = _particleSource.GetComponentInChildren<MeshCollider>();
            _uranCollider.transform.localScale = new Vector3((inheritedRadius) * 2, (inheritedRadius) * 2, (inheritedRadius) * 2);
        }
    }

    private void Update()
    {
        for (int i = 0; i < _records.Count; i++)
        {
            var record = _records[i];
            record.sourcePosition = this.transform.position;
            record.targetPosition = record.targetComponent.transform.position;
        }
        
        if(isPlayer == false)
        {
            _uranCollider.transform.localScale = new Vector3(inheritedRadius * 2, inheritedRadius * 2, inheritedRadius * 2);
            _sphereCollider.radius = inheritedRadius;
        }

        if (entityComponent != null && !entityComponent.isImmuneToRadiation)
        {
            entityComponent.Damage(inheritedDamage * Time.deltaTime, null);
        }
        
        inheritedDamage = damage;
        inheritedRadius = radius;
#if UNITY_EDITOR
        ProcessDebugSymbols();
#endif
    }

    private void OnTriggerEnter(Collider other)
    {
        var instanceID = other.gameObject.GetInstanceID();
        if(instanceID == this.gameObject.GetInstanceID()) return;
        if(_records.Any(x=>x.targetInstanceId == instanceID)
           || instanceID == this.gameObject.GetInstanceID()) return;
        if (other.gameObject.TryGetComponent<EntityComponent>(out _))
        {
            if (!other.gameObject.TryGetComponent<RadiationSourceComponent>(out var source))
            {
                source = other.gameObject.AddComponent<RadiationSourceComponent>();
                source.particles = particles;
            }
            _records.Add(_radiationSystem.Register(this, source));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var instanceID = other.gameObject.GetInstanceID();
        if(instanceID == this.gameObject.GetInstanceID()) return;

        if (other.gameObject.TryGetComponent<EntityComponent>(out _))
        {
            //radiationSourceComponent.FadeAway(fadeDuration);
            var obj = other.GetComponent<RadiationSourceComponent>();
            _records.Remove(_radiationSystem.Unregister(this.GetInstanceID(), obj.GetInstanceID()));
        }
    }

    private void OnDestroy()
    {
        foreach (var connection in _records)
        {
            _radiationSystem.Unregister(connection.sourceInstanceId, connection.targetInstanceId);
        }
        Destroy(_sphereCollider);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.2f, 0.2f, 0.2f, 0.2f);
        Gizmos.DrawSphere(transform.position, inheritedRadius);
    }
}

