using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
[Serializable]
public struct MainBoid : ISharedComponentData, IEquatable<MainBoid>
{
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
    public int hashArray(object hash)
    {
        var x = (IStructuralEquatable) hash;

        if (hash is int[])
        {
            return x.GetHashCode(EqualityComparer<int>.Default);
        }
        else if (hash is float[])
        {
            return x.GetHashCode(EqualityComparer<float>.Default);
        }
        else
        {
            Debug.LogError("can't classify the array you are attempting to hash, either add another option or change what you are putting in");
            return 0;
        }
    }

    public override int GetHashCode()
    {
        // TODO: write your implementation of GetHashCode() here

        var boidActionsHash = boidActions.GetHashCode();
        var nonBoidActionsHash = nonBoidActions.GetHashCode();

        float[] combination = { boidActionsHash, nonBoidActionsHash, group };

        var hashCode = hashArray(combination);

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
    public bool divideByNearby; //I think this means divide the effect by how many boids are nearby, usefully for stuff like cohesion to drop off as a  boid gets close to other boids
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