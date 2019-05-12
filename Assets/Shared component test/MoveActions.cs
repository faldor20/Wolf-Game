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
        public NativeArray<float> rotations;
        //   [Serializable]
        //   public DynamicBuffer<float> rotations;
        //  [Serializable]
        //  public DynamicBuffer<float> distances;
        public NativeArray<float> distances;
    }

[Serializable]
public enum Direction
{
    left = 1,
    right = 0
}
