using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

    [Serializable]
    public struct MoveActions : ISharedComponentData
    {

        public float firstDistance;
        [Range(-90f, 90f)]
        public float firstRotation;
        [Range(-90f, 90f)]
   //     public NativeArray<float> rotations;
    //    [Serializable]
    public float[] rotations;
    //   public DynamicBuffer<float> rotations;
    // [Serializable]
    public float[] distances;
    //  public DynamicBuffer<float> distances;
    //  public NativeArray<float> distances;
}

[Serializable]
public enum Direction
{
    left = 1,
    right = 0
}

public class MoveActionsComponent : SharedComponentDataProxy<MoveActions> { }