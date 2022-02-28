using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;
using static Unity.Mathematics.math;

[Obsolete]
public class RadiationSystem : MonoBehaviour
{
#if UNITY_EDITOR
    [Header("Readonly debug properties")]
    public int activeConnectionsAmount;
    public int standbyConnectionsAmount;

    private void ProcessDebugSymbols()
    {
        activeConnectionsAmount = _activeConnections.Count;
        standbyConnectionsAmount = _standbyConnections.Count;
    }
#endif
    [Header("Main properties")]
    private List<RadiationSourceData> _activeConnections = new();
    private List<RadiationSourceData> _standbyConnections = new();

    private JobHandle _lastScheludedHandle;
    private NativeArray<RaycastHit> _hits;
    private NativeArray<Multiplier> _multipliers;
    private NativeArray<RaycastCommand> _commands;
    private int _lenght;
    private bool _firstTime = true;
    private NativeArray<RadiationSourceDataUnmanaged> _radiationSourceDataUnmanageds;

    public RadiationSourceData Register(RadiationSourceComponent source, RadiationSourceComponent subject)
    {
        var radiationSourceData = new RadiationSourceData
        {
            sourceComponent = source,
            sourceInstanceId = source.GetInstanceID(),
            sourcePosition = source.transform.position,
            targetComponent = subject,
            targetInstanceId = subject.GetInstanceID(),
            targetPosition = subject.transform.position
        };
        _standbyConnections.Add(radiationSourceData);
        return radiationSourceData;
    }
    
    public RadiationSourceData Unregister(int sourceId, int subjectId)
    {
        RadiationSourceData first = new RadiationSourceData();
        var found = false;
        for (var i = 0; i<_activeConnections.Count; i++)
        {
            var x = _activeConnections[i];
            if (x.sourceInstanceId == sourceId && x.targetInstanceId == subjectId)
            {
                first = x;
                found = true;
                break;
            }
        }

        if (!found)
        {
            Debug.LogWarning("requested pair not found");
        }

        first.isDeleted = true;
        return first;
    }

    //this update function is VERY GC-heavy
    void Update()
    {
        //process raycasts scheluded by last batch
        if (!_firstTime)
        {
            _lastScheludedHandle.Complete();

            var hits = new NativeArray<Hit>(_hits.Length, Allocator.TempJob);
            for (var i = 0; i < _hits.Length;)
            {
                var raycastHit = _hits[i];
                if (raycastHit.colliderInstanceID == 0 || raycastHit.collider is null)
                {
                    i += 8 - (i % 8);
                    continue;
                }

                var hit = new Hit()
                {
                    exists = true,
                    distance = raycastHit.distance,
                    tag = new FixedString32Bytes(raycastHit.collider.tag)
                };
                hits[i] = hit;
                ++i;
            }
            
            var processor = new RaycastProcessor
            {
                hits = hits,
                multipliers = _multipliers,
                data = _radiationSourceDataUnmanageds
            };

            var processorHandle = processor.Schedule(_lenght, 2);
            processorHandle.Complete();
            
            hits.Dispose();

            var mps = processor.multipliers;
            if (mps.Length > 0 & _activeConnections.Count > 0)
            {
                for (var i = 0; i < mps.Length & i < _activeConnections.Count; ++i)
                {
                    var comp = _activeConnections[i];
                    if(comp.isDeleted) continue;
                    comp.targetComponent.inheritedDamage += comp.sourceComponent.damage * mps[i].damageMultiplier;
                    comp.targetComponent.inheritedRadius += comp.sourceComponent.radius * mps[i].radiusMultiplier;
                }
            }
        }
        
        //prepare new _activeConnections
        _activeConnections.RemoveAll(x => x.isDeleted);
        _activeConnections.AddRange(_standbyConnections);
        _standbyConnections.Clear();

        //prepare new batches
        _lenght = _activeConnections.Count;
        var radiationSources = _activeConnections.Select(x=>x.Unmanaged).ToArray(); //linq goes brrrr
        
        if(!_firstTime) _radiationSourceDataUnmanageds.Dispose();
        _radiationSourceDataUnmanageds = new NativeArray<RadiationSourceDataUnmanaged>(radiationSources, Allocator.TempJob);
        //collect position data
        var sourceTransforms = _activeConnections.Select(x => x.sourceComponent.transform);
        var sourcePositionGetter = new CollectSourcePositionData()
        {
            data = _radiationSourceDataUnmanageds
        };
        var transformAccessArray = new TransformAccessArray(sourceTransforms.ToArray());
        var sourcePositionHandle = sourcePositionGetter.ScheduleReadOnly(transformAccessArray, 16);
        
        var targetTransforms = _activeConnections.Select(x => x.targetComponent.transform);
        var targetPositionGetter = new CollectTargetPositionData()
        {
            data = _radiationSourceDataUnmanageds
        };
        var accessArray = new TransformAccessArray(targetTransforms.ToArray());
        var targetPositionHandle = targetPositionGetter.ScheduleReadOnly(accessArray, 16);
        
        var raycastCommands = new NativeArray<RaycastCommand>(_lenght, Allocator.TempJob);
        var batchesGenerator = new RaycastBatchesGenerator //create batch generator job
        {
            data = _radiationSourceDataUnmanageds,
            commands = raycastCommands
        };
        
        targetPositionHandle.Complete();
        sourcePositionHandle.Complete();
        transformAccessArray.Dispose();
        accessArray.Dispose();

        var batcherHandle = batchesGenerator.Schedule(_lenght, 4); //schelude raycast batch generator

        if(!_firstTime) _hits.Dispose();
        _hits = new NativeArray<RaycastHit>(_lenght * 8, Allocator.TempJob, //prepare data for raycasts
            NativeArrayOptions.UninitializedMemory);
        
        if(!_firstTime) _multipliers.Dispose();
        _multipliers = new NativeArray<Multiplier>(_lenght, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);

        batcherHandle.Complete(); //wait for batch preparations to complete
        
        if(!_firstTime) _commands.Dispose();
        _commands = batchesGenerator.commands;
        _lastScheludedHandle = RaycastCommand.ScheduleBatch(_commands,  //schelude raycasts
            _hits, 1);
        
        _firstTime = false;

#if UNITY_EDITOR
        ProcessDebugSymbols();
#endif
    }
    [BurstCompile]
    private struct RaycastBatchesGenerator : IJobParallelFor
    {
        
        [ReadOnly]
        public NativeArray<RadiationSourceDataUnmanaged> data;
        [WriteOnly]
        public NativeArray<RaycastCommand> commands;

        public void Execute(int index)
        {
            var source = data[index];
            var direction = source.targetPosition - source.sourcePosition;
            commands[index] = new RaycastCommand(source.sourcePosition, direction, length(direction), maxHits: 8);
        }
    }

    [BurstCompile]
    private struct RaycastProcessor : IJobParallelFor
    {
        [ReadOnly]
        public NativeArray<Hit> hits;
        
        [ReadOnly]
        public NativeArray<RadiationSourceDataUnmanaged> data;
        
        [WriteOnly]
        public NativeArray<Multiplier> multipliers;

        public void Execute(int index)
        {
            var multiplier = new Multiplier
            {
                damageMultiplier = 1,
                radiusMultiplier = 1
            };
            for (var i = 8*index; i < 8*index+8; i++)
            {
                var hit = hits[i];

                if (!hit.exists) break; //first invalid hit in RaycastCommand has null in collider
                // ReSharper disable Unity.BurstManagedString
                if (hit.tag == "ThinWall")
                {
                    multiplier.damageMultiplier *= 0.9f;
                    multiplier.radiusMultiplier -= ((data[index].maxRadius - hit.distance) * 0.1f) / data[index].maxRadius;
                }
                else if (hit.tag == "MediumWall")
                {
                    multiplier.damageMultiplier *= 0.6f;
                    multiplier.radiusMultiplier -= ((data[index].maxRadius - hit.distance) * 0.4f) / data[index].maxRadius;
                }
                else if (hit.tag == "ThickWall")
                {
                    multiplier.damageMultiplier *= 0.1f;
                    multiplier.radiusMultiplier -= ((data[index].maxRadius - hit.distance) * 0.6f) / data[index].maxRadius;
                }
                else if (hit.tag == "BlockingWall")
                {
                    multiplier.damageMultiplier = 0f;
                    break;
                }
            }

            var magnitude = length(data[index].targetPosition - data[index].sourcePosition);
            multiplier.damageMultiplier *= 1 - (magnitude / (data[index].maxRadius));
            multiplier.radiusMultiplier *= 1 - (magnitude / (data[index].maxRadius));

            if (magnitude > data[index].maxRadius)
            {
                multiplier.damageMultiplier = 0;
                multiplier.radiusMultiplier = 0;
            }

            multipliers[index] = multiplier;
        }
    }
    
    [BurstCompile]
    public struct CollectSourcePositionData : IJobParallelForTransform
    {
        [NativeDisableParallelForRestriction, NativeDisableContainerSafetyRestriction]
        public NativeArray<RadiationSourceDataUnmanaged> data;

        public void Execute(int index, TransformAccess transform)
        {
            var sourceData = data[index];
            sourceData.sourcePosition = transform.position;
            data[index] = sourceData;
        }
    }
    
    [BurstCompile]
    public struct CollectTargetPositionData : IJobParallelForTransform
    {
        [NativeDisableParallelForRestriction, NativeDisableContainerSafetyRestriction]
        public NativeArray<RadiationSourceDataUnmanaged> data;

        public void Execute(int index, TransformAccess transform)
        {
            var sourceData = data[index];
            sourceData.targetPosition = transform.position;
            data[index] = sourceData;
        }
    }

    private void OnDestroy()
    {
        //clean used resources
        _lastScheludedHandle.Complete();
        _commands.Dispose();
        _hits.Dispose();
        _multipliers.Dispose();
        _radiationSourceDataUnmanageds.Dispose();
    }

    public class RadiationSourceData
    {
        public float3 sourcePosition;
        public int sourceInstanceId;
        public RadiationSourceComponent sourceComponent;
        
        public float3 targetPosition;
        public int targetInstanceId;
        public RadiationSourceComponent targetComponent;

        public float maxRadius => sourceComponent.radius + sourceComponent.inheritedRadius;

        public bool isDeleted; 

        public RadiationSourceDataUnmanaged Unmanaged => new RadiationSourceDataUnmanaged
        {
            sourceInstanceId = sourceInstanceId,
            targetInstanceId = targetInstanceId,
            sourcePosition = sourcePosition,
            targetPosition = targetPosition,
            maxRadius = maxRadius
        };
    }
    
    public struct RadiationSourceDataUnmanaged //not unmanaged, but doesnt contain ref types
    {
        public float3 sourcePosition;
        public int sourceInstanceId;

        public float3 targetPosition;
        public int targetInstanceId;

        public float maxRadius;
    }

    private struct Multiplier
    {
        public float damageMultiplier;
        public float radiusMultiplier;
    }

    private struct Hit
    {
        public bool exists;
        public float distance;
        public FixedString32Bytes tag;
    }
}
