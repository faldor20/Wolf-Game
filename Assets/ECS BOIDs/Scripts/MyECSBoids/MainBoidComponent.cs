using System;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct MainBoid : ISharedComponentData
{
    public float seperationMaxDistance;
    public float separationWeight;
    public float seperationrange;
    public bool scaleSeperationWeight;

    public float alignmentMaxDistance;
    public float alignmentWeight;
    public float alignmentRange;
    public bool scaleAlignmentWeight;

    public float targetMaxDistance;
    public float targetWeight;
    public bool scaleTargetWeight;

    public float predatorFleeDistance;
    public float predatorFleeMaxDistance;

    public float group;

    public float3 heading;
}

public class MainBoidComponent : SharedComponentDataWrapper<MainBoid> { }