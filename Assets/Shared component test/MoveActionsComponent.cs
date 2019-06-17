using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public struct MoveActions : ISharedComponentData, IEquatable<MoveActions>
{

    public float firstDistance;
    [Range(-90f, 90f)]
    public float firstRotation;

    public float[] rotations;
    public float[] distances;
#region OptimizedEquality
    /*
        public bool Equals(MoveActions other)
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
            if (toBeHashed == null)
            {
                return 0;
            }
            return ((IStructuralEquatable) toBeHashed).GetHashCode(EqualityComparer<float>.Default);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here

            var rotationsHash = GetfloatArrayHash(rotations);
            var distancesHash = GetfloatArrayHash(distances);
            float[] combination = { rotationsHash, distancesHash, firstDistance, firstRotation };
            var hashCode = ((IStructuralEquatable) combination).GetHashCode(EqualityComparer<float>.Default);;

            return hashCode.GetHashCode();
        }*/
#endregion

#region LazyEquality
    public bool Equals(MoveActions other)
    { //TODO find out a way so that it doesnt run this comparison every update, becuase thats real;y slow
        if (GetType() != other.GetType())
        {
            return false;
        }
        //Debug.Log(typeof(string).Assembly.ImageRuntimeVersion);

        // I cant use a tuple here because a tuple containg an array connot be compared
        return (firstDistance, firstRotation, GetfloatArrayHash(rotations), GetfloatArrayHash(distances)) == (other.firstDistance, other.firstRotation, GetfloatArrayHash(other.rotations), GetfloatArrayHash(other.distances));
    }
    private int GetfloatArrayHash(float[] toBeHashed)
    {
        if (toBeHashed == null)
        {
            return 0;
        }
        return ((IStructuralEquatable) toBeHashed).GetHashCode(EqualityComparer<float>.Default);
    }
    public override int GetHashCode()
    {
        var hashCode = (firstDistance, firstRotation, rotations, distances).GetHashCode();
        return hashCode;

    }
#endregion

}

[Serializable]
public struct MoveData
{
    public float rotation;
    public float distance;
}

[Serializable]
public enum Direction
{
    left = 1,
    right = 0
}
//public class MoveActionsComponent : SharedComponentDataProxy<MoveActions> { }