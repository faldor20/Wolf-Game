using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[RequiresEntityConversion]
public class MoveActionsArraysProxy : MonoBehaviour, IConvertGameObjectToEntity
{
    public float[] Rotations;
    public float[] Distances;

    // The MonoBehaviour data is converted to ComponentData on the entity.
    // We are specifically transforming from a good editor representation of the data 
    // To a good runtime representation 

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddSharedComponentData(entity, new MoveActionsArrays
        {
            rotations = Rotations,
            distances = Distances
        });
    }

}