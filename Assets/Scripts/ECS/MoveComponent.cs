using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
public struct Move : IComponentData
{
    public float TurnSpeed;
    public float MoveSpeed;
}
public class MoveComponent : ComponentDataProxy<Move> { }