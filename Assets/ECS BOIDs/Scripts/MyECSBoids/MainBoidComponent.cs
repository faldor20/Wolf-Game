using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
[Serializable]
public struct MainBoid : ISharedComponentData, IEquatable<MainBoid>
{
    private int hashCode;

    public BoidAction[] boidActions;
    public NonBoidAction[] nonBoidActions;
    public int group;

    public bool Equals(MainBoid other)
    {
        if (GetType() != other.GetType())
        {
            return false;
        }

        // TODO: write your implementation of Equals() here
        var a = this.GetHashCode();
        var b = other.GetHashCode();

        return (a == b);
    }

    private int GetintArrayHash(int[] toBeHashed)
    {
        return ((IStructuralEquatable) toBeHashed).GetHashCode(EqualityComparer<float>.Default);
    }

    public override int GetHashCode()
    {
        // TODO: write your implementation of GetHashCode() here
        if (hashCode == 0)
        {
            var boidActionsHash = boidActions.GetHashCode();
            var nonBoidActionsHash = nonBoidActions.GetHashCode();

            float[] combination = { boidActionsHash, nonBoidActionsHash, group };

            hashCode = ((IStructuralEquatable) combination).GetHashCode(EqualityComparer<float>.Default);
        }

        return hashCode.GetHashCode();
    }
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