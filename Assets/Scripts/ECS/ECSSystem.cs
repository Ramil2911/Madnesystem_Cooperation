using System;
using UnityEngine;

//WIP
namespace ECS
{
    public abstract class ECSSystem
    {
        public abstract void Setup();
        public abstract void Update();
    }

    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    sealed class Inject : Attribute
    {
        public Inject()
        {
            
        }
    }
}