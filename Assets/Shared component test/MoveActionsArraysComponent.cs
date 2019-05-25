using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public struct MoveActionsArrays : ISharedComponentData,ISupplyingArrayData
{
    public float[] rotations;
    public float[] distances;
}
