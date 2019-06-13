using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
[Serializable]
public struct MoveActionsArrays : ISharedComponentData, ISupplyingArrayData, IEquatable<MoveActionsArrays>
{

    private int hashCode;

    public float[] rotations;
    public float[] distances;

    // override object.Equals
    public bool Equals(MoveActionsArrays other)
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

    private int GetfloatArrayHash(float[] toBeHashed)
    {
        return ((IStructuralEquatable) toBeHashed).GetHashCode(EqualityComparer<float>.Default);
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        // TODO: write your implementation of GetHashCode() here
        if (hashCode == 0)
        {
            var rotationsHash = GetfloatArrayHash(rotations);
            var distancesHash = GetfloatArrayHash(distances);
            hashCode = distancesHash ^ rotationsHash;
        }

        return hashCode.GetHashCode();
    }

}