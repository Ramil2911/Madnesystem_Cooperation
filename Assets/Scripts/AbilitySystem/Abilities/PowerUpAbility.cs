using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;
using static Unity.Mathematics.math;

[AbilityId(4)]
public unsafe class PowerUpAbility : Ability
{
    public override float DurationRemaining { get; set; }
    public override float Duration { get; set; }
    public override string Name { get; set; }

    private int oldSize = -1;

    private SphereCollider _sphereCollider;
    private GameObject _particleSource;
    private MeshCollider _uranCollider;

    private MainThreadDispatcher mainThreadDispatcher;

    private bool isPlayer;
    private NativeArray<RaycastHit> _hits;
    private NativeArray<RaycastCommand> _commands;
    private NativeArray<Translation> _translations;
    private NativeArray<RaycastHitBurst> _hitsBurst;
    private NativeArray<PowerUpDataPtr> _powerUpDataPtrs;
    private NativeArray<MainThreadFunction> _getLayersResults;
    private NativeArray<UnsafeArg> _getLayersArguments;
    private NativeArray<MainThreadFunctionPtr> _queueArray;
    

    public PowerUpData data;
    public List<PowerUpDataPtr> impactedData = new();
    
    public List<PowerUpAbility> impactedByRefData = new();
    public List<PowerUpAbility> impactedRefData = new();

    public override void OnStart()
    {
        mainThreadDispatcher = GameObject.FindWithTag("MainThreadDispatcher").GetComponent<MainThreadDispatcher>();
        
        if(base.transform.CompareTag("Player"))
        {
            isPlayer = true;
        }
        _sphereCollider = base.transform.gameObject.AddComponent<SphereCollider>();
        _sphereCollider.radius = data.radius;
        _sphereCollider.isTrigger = true;
    }
    
    
    public override JobHandle? OnUpdateScheduleJob(JobHandle? handle)
    {

        mainThreadDispatcher.Process(); //ensure queue is empty to prevent deadlock
        data.inheritedDamage = 0;
        data.inheritedDuration = 0;
        data.inheritedRadius = 0;
        
        if (impactedData.Count != oldSize)
        {
            //save new old value
            oldSize = impactedData.Count;
            
            //clean memory
            _hits.Dispose();
            _commands.Dispose();
            _translations.Dispose();
            _hitsBurst.Dispose();
            _powerUpDataPtrs.Dispose();
            _queueArray.Dispose();
            _getLayersArguments.Dispose();
            _powerUpDataPtrs.Dispose();
            
            //allocate new
            _hits = new NativeArray<RaycastHit>(oldSize * 8, Allocator.Persistent, //prepare data for raycasts
                NativeArrayOptions.UninitializedMemory);
            _hitsBurst = new NativeArray<RaycastHitBurst>(oldSize * 8, Allocator.Persistent,
                NativeArrayOptions.UninitializedMemory);
            _commands = new NativeArray<RaycastCommand>(oldSize, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            _translations =
                new NativeArray<Translation>(oldSize, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            _powerUpDataPtrs = new NativeArray<PowerUpDataPtr>(impactedData.GetInternalArray(), Allocator.Persistent);
            _getLayersResults = new NativeArray<MainThreadFunction>(oldSize * 8, Allocator.Persistent,
                NativeArrayOptions.UninitializedMemory);
            _getLayersArguments = new NativeArray<UnsafeArg>(oldSize * 8, Allocator.Persistent,
                NativeArrayOptions.UninitializedMemory);
            _queueArray = new NativeArray<MainThreadFunctionPtr>(oldSize * 8, Allocator.Persistent,
                NativeArrayOptions.UninitializedMemory);

        }

        //prepare transforms
        var transforms = new Transform[oldSize];
        for (var i = 0; i < transforms.Length; ++i)
        {
            transforms[i] = impactedRefData[i].transform;
        }
        var transformAccessArray = new TransformAccessArray(transforms);
        
        //create "graph"
        var handle1 = new CopyTranslationFromTransform(_translations)
            .Schedule(transformAccessArray);
        var handle2 = new GenerateRaycastCommands(transform.position, _translations, _commands)
            .Schedule(oldSize, 8, handle1);
        var handle3 = RaycastCommand.ScheduleBatch(_commands, _hits, 8, handle2);
        var handle4 = new RaycastHitToBurstHit(_hits, _hitsBurst).Schedule(oldSize * 8, 8, handle3);
        var handle5 = new GetLayersOnMainThread(_hits, mainThreadDispatcher.actionQueue, _getLayersArguments,
            _getLayersResults, _queueArray).Schedule(oldSize*8, 8, handle4);
        var handle6 = new WaitForMainThread(_queueArray).Schedule(handle5);
        return new UpdatePowerUpData(data.damage, data.radius, transform.position, _powerUpDataPtrs, _hitsBurst, _translations, _queueArray)
            .Schedule(transformAccessArray, handle6);
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<AbilityComponent>(out var comp)
            && !other.gameObject.TryGetComponent<EntityComponent>(out var colliderEntityComponent)
            && !colliderEntityComponent.isPlayer)
        {
            var ability = comp.AddOrGet<PowerUpAbility>();
            ability.impactedByRefData.Add(this);
            ability.impactedRefData.Add(ability);
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<AbilityComponent>(out var comp)
            && !other.gameObject.TryGetComponent<EntityComponent>(out var colliderEntityComponent)
            && !colliderEntityComponent.isPlayer)
        {
            var ability = comp.AddOrGet<PowerUpAbility>();
            if(ability == null) return;
            ability.impactedByRefData.Remove(this);
            impactedRefData.Remove(ability);
        }
    }

    public override void OnDestroy()
    {
        _hits.Dispose();
        _commands.Dispose();
        _translations.Dispose();
        _hitsBurst.Dispose();
        _powerUpDataPtrs.Dispose();
        _queueArray.Dispose();
        _getLayersArguments.Dispose();
        _powerUpDataPtrs.Dispose();
    }

    public struct Translation
    {
        public float3 position;
        public quaternion rotation;
    }

    [BurstCompile]
    public struct CopyTranslationFromTransform : IJobParallelForTransform
    {
        [WriteOnly]
        private NativeArray<Translation> translations;

        public CopyTranslationFromTransform(NativeArray<Translation> translations)
        {
            this.translations = translations;
        }
        public void Execute(int index, TransformAccess transform)
        {
            translations[index] = new Translation()
            {
                position = transform.position,
                rotation = transform.rotation
            };
        }
    }
    
    [BurstCompile]
    private struct GenerateRaycastCommands : IJobParallelFor
    {
        [ReadOnly] 
        private float3 original;
        
        [ReadOnly]
        private NativeArray<Translation> data;
        
        [WriteOnly]
        private NativeArray<RaycastCommand> commands;

        public GenerateRaycastCommands(float3 original, NativeArray<Translation> data,
            NativeArray<RaycastCommand> commands)
        {
            this.original = original;
            this.data = data;
            this.commands = commands;
        }

        public void Execute(int index)
        {
            var source = data[index];
            var direction = source.position - original;
            commands[index] = new RaycastCommand(original, direction, length(direction),
                maxHits: 8);
        }
    }

    [BurstCompile]
    private struct RaycastHitToBurstHit : IJobParallelFor
    {
        [ReadOnly]
        private NativeArray<RaycastHit> hits;
        [WriteOnly]
        private NativeArray<RaycastHitBurst> hitsBurst;

        public RaycastHitToBurstHit(NativeArray<RaycastHit> hits, NativeArray<RaycastHitBurst> hitsBurst)
        {
            this.hits = hits;
            this.hitsBurst = hitsBurst;
        }
        
        public void Execute(int index)
        {
            hitsBurst[index] = hits[index].ToStruct();
        }
    }
    
    private struct GetLayersOnMainThread : IJobParallelFor
    {
        
        [ReadOnly]
        private NativeArray<RaycastHit> hits;
        
        private NativeQueue<MainThreadFunctionPtr> queue;
        private NativeArray<MainThreadFunctionPtr> queueArray;
        private NativeArray<MainThreadFunction> results;
        private NativeArray<UnsafeArg> args;

        public GetLayersOnMainThread(NativeArray<RaycastHit> raycastHits, NativeQueue<MainThreadFunctionPtr> queue,
            NativeArray<UnsafeArg> args, NativeArray<MainThreadFunction> results, NativeArray<MainThreadFunctionPtr> queueArray)
        {
            hits = raycastHits;
            this.queue = queue;
            this.args = args;
            this.results = results;
            this.queueArray = queueArray;
        }
        
        public void Execute(int index)
        {
            args[index] = new UnsafeArg()
            {
                hit = hits[index]
            };
            results[index] = new MainThreadFunction()
            {
                func = &GetLayer,
                args = (IntPtr)args.GetUnsafePtr()+index*sizeof(UnsafeArg), //some funny pointer math, i hope it wont break anything
            };
            queueArray[index] = new MainThreadFunctionPtr()
            {
                ptr = (MainThreadFunction*) results.GetUnsafePtr()+index*sizeof(MainThreadFunction)
            };
            queue.Enqueue(queueArray[index]);
        }
        
        public static IntPtr GetLayer(IntPtr data)
        {
            var args = *(UnsafeArg*) data;
            return (IntPtr)args.hit.transform.gameObject.layer;
        }
    }
    
    [BurstCompile]
    private struct WaitForMainThread : IJob
    {
        [ReadOnly]
        private NativeArray<MainThreadFunctionPtr> results;

        public WaitForMainThread(NativeArray<MainThreadFunctionPtr> results)
        {
            this.results = results;
        }
        
        public void Execute()
        {
            for (var i = 0; i < results.Length; ++i)
            {
                while (results[i].ptr->isCompleted != true)
                {
                    //pass
                }
            }
        }
    }
    
    [BurstCompile]
    private struct UpdatePowerUpData : IJobParallelForTransform
    {
        [ReadOnly]
        private NativeArray<PowerUpDataPtr> powerUpDataPtrs;
        [ReadOnly]
        private NativeArray<RaycastHitBurst> raycastHits;
        [ReadOnly]
        private NativeArray<Translation> translations;
        [ReadOnly] 
        private NativeArray<MainThreadFunctionPtr> layerData;


        private float baseDamage;
        private float baseRadius;
        private float3 basePosition;

        public UpdatePowerUpData(float baseDamage, float baseRadius, float3 basePosition,
            NativeArray<PowerUpDataPtr> powerUpDataPtrs, NativeArray<RaycastHitBurst> raycastHit,
            NativeArray<Translation> translations, NativeArray<MainThreadFunctionPtr> layersData)
        {
            this.powerUpDataPtrs = powerUpDataPtrs;
            this.raycastHits = raycastHit;
            this.baseDamage = baseDamage;
            this.baseRadius = baseRadius;
            this.basePosition = basePosition;
            this.translations = translations;
            this.layerData = layersData;
        }

        public void Execute(int index, TransformAccess transform)
        {
            var powerUpData = powerUpDataPtrs[index].value;
            var multiplier = new Multiplier()
            {
                damageMultiplier = 1,
                radiusMultiplier = 1
            };
            for (var i = 8 * index; i < 8 * index + 8;)
            {
                var hit = raycastHits[i];
                if(hit.m_ColliderID == 0)
                {
                    i += 8 - (i % 8);
                    continue;
                }
                
                var data = *layerData[i].ptr;

                if (((int)data.result & 2^7)==(2 ^ 7))
                {
                    multiplier.damageMultiplier *= 0.9f;
                    multiplier.radiusMultiplier -= ((baseRadius - hit.m_Distance) * 0.1f) / baseRadius;
                }
                else if (((int)data.result & 2^8)==(2^8))
                {
                    multiplier.damageMultiplier *= 0.6f;
                    multiplier.radiusMultiplier -= ((baseRadius - hit.m_Distance) * 0.4f) / baseRadius;
                }
                else if (((int)data.result & 2^9)==(2^9))
                {
                    multiplier.damageMultiplier *= 0.1f;
                    multiplier.radiusMultiplier -= ((baseRadius - hit.m_Distance) * 0.6f) / baseRadius;
                }
                else if (((int)data.result & 2^10)==(2^10))
                {
                    multiplier.damageMultiplier = 0f;
                    break;
                }
                
                ++i;
            }

            powerUpData->inheritedDamage += baseDamage * multiplier.damageMultiplier;
            powerUpData->inheritedRadius += baseRadius * multiplier.radiusMultiplier;
            
            var magnitude = length(translations[index].position - basePosition);
            if (magnitude < baseRadius)
            {
                powerUpData->inheritedDamage += 1 - (magnitude / baseRadius);
                powerUpData->inheritedRadius += 1 - (magnitude / baseRadius);
            }
        }

        private struct Multiplier
        {
            public float damageMultiplier;
            public float radiusMultiplier;
        }
    }
}

public struct PowerUpData
{
    public float radius;
    public float damage;
    public float fadeDuration; //tbd
    
    public float inheritedRadius;
    public float inheritedDamage;
    public float inheritedDuration;//tbd
}

public unsafe struct PowerUpDataPtr
{
    public PowerUpData* value;
}

public struct UnsafeArg
{
    public RaycastHit hit;
}