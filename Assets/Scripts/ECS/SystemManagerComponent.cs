using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ECS;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

//WIP
public unsafe class SystemManagerComponent : MonoBehaviour
{
    private Dictionary<long, ComponentDataType> typesById = new();
    private Dictionary<string, ComponentDataType> typesByName = new();
    private Dictionary<int, Entity> entities;
    private ECSSystem[] systems;
    void Start()
    {
        long i = 0;
        foreach (var type in AppDomain.CurrentDomain.GetAssemblies()
                     .SelectMany(x => x.GetTypes())
                     .Where(x => x.IsAssignableFrom(typeof(ECSComponent))))
        {
            var componentList = new NativeList<PtrVoid>(allocator: Allocator.Persistent);
            var typedef = new ComponentDataType()
            {
                id = i,
                components = componentList
            };
            typesById.Add(i, typedef);
            typesByName.Add(type.Name, typedef);
        }

        systems = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => x.IsAssignableFrom(typeof(ECSSystem)) && !x.IsStatic() && !x.IsAbstract)
            .Select(x => (ECSSystem)Activator.CreateInstance(x))
            .ToArray();

        foreach (var system in systems)
        {
            PrepareSystem(system);
        }
    }

    private void PrepareSystem(ECSSystem system)
    {
        foreach (var fieldInfo in system.GetType().GetFields())
        {
            if (Attribute.IsDefined(fieldInfo, typeof(Inject)))
            {
                if (fieldInfo.FieldType == typeof(NativeArray<>))
                {
                    var arg = typesByName[fieldInfo.FieldType.Name].components.AsArray();
                    var reference = __makeref(arg);
                    fieldInfo.SetValueDirect(reference, arg);
                }
                else if(fieldInfo.FieldType == typeof(GameObject))
                {

                }
            }
        }
    }


    void Update()
    {
        for (int i = 0; i < systems.Length; i++)
        {
            systems[i].Update();
        }
    }

    public void AddSystem(ECSSystem system)
    {
        PrepareSystem(system);
        Array.Resize(ref systems, systems.Length);
        systems[-1] = system;
    }
}

public struct ComponentDataType
{
    public long id;
    public NativeList<PtrVoid> components; 
}

public struct Entity
{
    public int id;
    public NativeList<PtrVoid> components;
    public GameObject go;
}
