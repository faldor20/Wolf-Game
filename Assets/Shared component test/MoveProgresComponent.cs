using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
[Serializable]
public struct MoveProgress : ISharedComponentData
{
float waitTimer;
}
public class MoveProgressComponent : SharedComponentDataProxy<MoveProgress> { }