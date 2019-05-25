using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
[Serializable]
public struct MoveProgress : IComponentData
{
public int stepsCompleted;
public float distanceRemaining;
public float waitTimer;
}
public class MoveProgressComponent : ComponentDataProxy<MoveProgress> { }