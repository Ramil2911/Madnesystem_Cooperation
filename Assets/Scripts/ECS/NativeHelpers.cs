using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
// ReSharper disable InconsistentNaming

namespace ECS
{
    public static unsafe class NativeHelpers
    {
        public static uint unmanaged_sizeof<T>() where T : unmanaged
            => (uint) UnsafeUtility.SizeOf<T>();

        public static Ptr<T> malloc<T>(Allocator allocator, uint size = 1) where T : unmanaged 
            => new(UnsafeUtility.Malloc(size, 4, allocator));
        
        public static void free<T>(Ptr<T> ptr, Allocator allocator) where T : unmanaged
            => UnsafeUtility.Free(ptr, allocator);
    }
    
    
}

public unsafe struct Ptr<T> where T : unmanaged
{
    public T* ptr;

    public Ptr(void* ptr)
    {
        this.ptr = (T*)ptr;
    }
    
    public Ptr(T* ptr)
    {
        this.ptr = ptr;
    }
    
    public Ptr(IntPtr ptr)
    {
        this.ptr = (T*)ptr.ToPointer();
    }

    public static implicit operator T*(Ptr<T> ptr)
        => ptr.ptr;
    
    public static explicit operator Ptr<T>(PtrVoid ptr)
        => new(ptr.ptr);
    
    public static explicit operator void*(Ptr<T> ptr)
        => ptr.ptr;

    public static explicit operator IntPtr(Ptr<T> ptr)
        => new(ptr.ptr);
}

public unsafe struct PtrVoid
{
    public void* ptr;

    public PtrVoid(void* ptr)
    {
        this.ptr = ptr;
    }

    public PtrVoid(IntPtr ptr)
    {
        this.ptr = (void*)ptr.ToPointer();
    }
    public static explicit operator IntPtr(PtrVoid ptr)
        => new IntPtr(ptr.ptr);
}