﻿using System;
using Unity.Entities;

[Serializable]
public struct Boid : ISharedComponentData
{
    public float cellRadius;
    public float separationWeight;
    public float alignmentWeight;
    public float targetWeight;
    public float obstacleAversionDistance;
    public float group;
}

public class BoidComponent : SharedComponentDataProxy<Boid> { }