using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[RequiresEntityConversion]
public class MoveActionsProxy : MonoBehaviour, IConvertGameObjectToEntity
{
    public float FirstDistance;
    [Range(-90f, 90f)]
    public float FirstRotation;
    [Range(-90f, 90f)]
    public float[] Rotations;
    public float[] Distances;

    // The MonoBehaviour data is converted to ComponentData on the entity.
    // We are specifically transforming from a good editor representation of the data 
    // To a good runtime representation 

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        Debug.Log(Rotations.Length);
       // var outRotations = new NativeArray<float>(Rotations, Allocator.Persistent);
       // Debug.Log("outrotations: " + MoveActionsSystem.ArrayToString(outRotations));
        dstManager.AddSharedComponentData(entity, new MoveActions
        {
            firstDistance = FirstDistance,
                firstRotation = FirstRotation,
                rotations = Rotations,
                distances = Distances
        });
      //  outRotations.Dispose();
    }

}