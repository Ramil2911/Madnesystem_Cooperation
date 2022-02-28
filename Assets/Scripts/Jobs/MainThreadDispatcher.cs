using System;
using Unity.Collections;
using UnityEngine;

//WIP
public class MainThreadDispatcher : MonoBehaviour
{
    public NativeQueue<MainThreadFunctionPtr> actionQueue = new NativeQueue<MainThreadFunctionPtr>();

    // Update is called once per frame
    void Update()
    {
        Process();
    }

    private void FixedUpdate()
    {
        Process();
    }

    private void LateUpdate()
    {
        Process();
    }

    public unsafe void Process()
    {
        while (!actionQueue.IsEmpty())
        {
            var ptr = actionQueue.Dequeue().ptr;
            ptr->result = ptr->func(ptr->args);
            ptr->isCompleted = true;
        }
    }
}

public unsafe struct MainThreadFunctionPtr
{
    public MainThreadFunction* ptr;
}

public unsafe struct MainThreadFunction
{
    public delegate* managed<IntPtr, IntPtr> func;
    public IntPtr args;
    public IntPtr result;
    public bool isCompleted;
}
