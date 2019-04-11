using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
public struct ECSPhysics : IComponentData
{
    public float3 velocity;
    public float drag;
}
public class ECSPhysicsComponent : ComponentDataProxy<ECSPhysics> { }