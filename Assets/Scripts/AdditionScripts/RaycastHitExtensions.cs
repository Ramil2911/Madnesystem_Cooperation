using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Burst;
using UnityEngine;

public static class RaycastHitExtensions
{
    [BurstCompile]
    public static unsafe RaycastHitBurst ToStruct(this RaycastHit hit)
    => *(RaycastHitBurst*) &hit;
}

    [StructLayout(LayoutKind.Sequential)]
public struct RaycastHitBurst
{
    public Vector3 m_Point;
    public Vector3 m_Normal;
    public int m_FaceID;
    public float m_Distance;
    public Vector2 m_UV;
    public int m_ColliderID;
}