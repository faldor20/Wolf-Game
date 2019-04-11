using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
[Serializable]
public struct MainBoid : ISharedComponentData
{

    public NativeArray<BoidAction> boidActions;
    public NativeArray<NonBoidAction> nonBoidActions;
    public int group;

}

public class MainBoidComponent : SharedComponentDataProxy<MainBoid> { }

[Serializable]
public struct BoidAction
{
    public BoidActionType actionType;
    public float range;
    public float weight;
    public bool divideByNearby;
    public float viewangle;
}

[Serializable]
public struct NonBoidAction
{
    public NonBoidActionType actionType;
    public float range;
    public float weight;
    public bool divideByNearby;
    public float viewangle;
}
//Actions which involve checking other boids
[Serializable]
public enum BoidActionType
{
    Alignment,
    Cohesion,
    Seperation,
}

//Actions which do not involve checking other boids
[Serializable]
public enum NonBoidActionType
{
    Fleeing,
    Targeting
}