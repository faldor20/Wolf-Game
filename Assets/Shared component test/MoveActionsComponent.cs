using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public struct MoveActions : ISharedComponentData
{

    public float firstDistance;
    [Range (-90f, 90f)]
    public float firstRotation;

  public float[] rotations;
    public float[] distances;

   // public MoveData[] actions;

}

[Serializable]
public struct MoveData
{
    public float rotation;
    public float distance;
}

[Serializable]
public enum Direction
{
    left = 1,
    right = 0
}
//public class MoveActionsComponent : SharedComponentDataProxy<MoveActions> { }