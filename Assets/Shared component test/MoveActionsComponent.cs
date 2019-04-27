using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct MoveActions : ISharedComponentData
{

    public float firstDistance;
    public Direction firstDirection;
    public Direction[] directions;
    public float[] distances;
}

[Serializable]
public enum Direction
{
    left = 1,
    right = 0
}
public class MoveActionsComponent : SharedComponentDataProxy<MoveActions> { }