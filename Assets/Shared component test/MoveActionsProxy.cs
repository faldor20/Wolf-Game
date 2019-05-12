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
        // We are specifically transforming from a good editor representation of the data (Represented in degrees)
        // To a good runtime representation (Represented in radians)

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {

            dstManager.AddSharedComponentData(entity, new MoveActions
            {
                firstDistance = FirstDistance,
                firstRotation = FirstRotation,
                rotations = new NativeArray<float>(Rotations, Allocator.Persistent),
                distances = new NativeArray<float>(Distances, Allocator.Persistent)
            });
            // dstManager.AddComponentData(entity, new LifeTime { Value = 0.0F });
        }

    }
