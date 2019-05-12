using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

//This system is somewhat poorly named.
// It actually sets the rotation from the heading.
//the heading is set in a nother system
// why does this system need to exist? could the rotation not have been set directly?
public class MoveActionsSystem : JobComponentSystem
{

    public List<MoveActions> m_UniqueTypes = new List<MoveActions> (10);

    public NativeHashMap<Entity, StoreableEntityData> m_storeableEntityData = new NativeHashMap<Entity, StoreableEntityData> ();

    BeginInitializationEntityCommandBufferSystem m_EntityCommandBufferSystem;
    protected override void OnCreate ()
    { //This is our barrier system
        // Cache the BeginInitializationEntityCommandBufferSystem in a field, so we don't have to create it every frame
        m_EntityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem> ();

        m_MoveActionsGroup = GetEntityQuery (new EntityQueryDesc
        {
            All = new [] { ComponentType.ReadOnly<MoveActions> (), ComponentType.ReadWrite<Translation> () },
                Options = EntityQueryOptions.Default
        });
    }

    public struct StoreableEntityData
    {
        public float waitTimer;
        public int stepsCompleted;
        public int distanceRemaining;
    }

    [BurstCompile]
    [RequireComponentTag (typeof (MoveActions))]
    struct ExecuteActions : IJobForEachWithEntity<Translation, Rotation>
    {
        public float deltaTime;
       [ReadOnly] public NativeHashMap<Entity, StoreableEntityData> storableEntityData;
        [ReadOnly] public MoveActions moveActions;
      [ReadOnly] public EntityCommandBuffer CommandBuffer;

        public NativeArray<Translation> postion;
        public void Execute (Entity entity, int index, ref Translation position, ref Rotation rotation)
        {
           /*  var storedData = storableEntityData[entity];
            if (storedData.waitTimer <= 0)
            {
                if (storedData.distanceRemaining > 0)
                {
                    position.Value = Move (position.Value, math.rotate (rotation.Value, Vector3.forward), deltaTime);
                }
                else
                if (storedData.stepsCompleted < moveActions.rotations.Length)
                {
                    rotation.Value = math.mul (math.normalize (rotation.Value), quaternion.AxisAngle (math.up (), moveActions.rotations[storedData.stepsCompleted]));
                }
                else
                {
                    CommandBuffer.RemoveComponent<RandomInitialHeading> (entity);
                }

            }
            else
            {
                storedData.waitTimer -= Time.deltaTime;
            } */

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="movedirecion"> A unit vector pointing in the direction of the movement to take place in world space</param>
        /// <param name="dt"> Delta time</param>
        public float3 Move (float3 currentPos, float3 moveDirection, float dt)
        {
            var newPos = currentPos + (moveDirection * dt);
            return newPos;
        }
    }

    private EntityQuery m_MoveActionsGroup;

    protected override JobHandle OnUpdate (JobHandle inputDeps)
    {
        EntityManager.GetAllUniqueSharedComponentData (m_UniqueTypes);
        Debug.Log (m_UniqueTypes.Count);
        for (int i = 0; i < m_UniqueTypes.Count; i++)
        {
            m_MoveActionsGroup.SetFilter (m_UniqueTypes[i]);
            var translation = m_MoveActionsGroup.ToComponentDataArray<Translation> (Allocator.TempJob, out JobHandle getTranslationJobHandle);
            var combinedHandles = JobHandle.CombineDependencies (inputDeps, getTranslationJobHandle);
            var moveActions = m_UniqueTypes[i];
            ExecuteActions executeActionsJob = new ExecuteActions
            {
                deltaTime = Time.deltaTime,
                storableEntityData = m_storeableEntityData,
                postion = translation,
                moveActions = m_UniqueTypes[i],
                CommandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer ()

            };
            JobHandle executeActionsJobHandle = executeActionsJob.Schedule (this, getTranslationJobHandle);
            inputDeps = getTranslationJobHandle;

            m_EntityCommandBufferSystem.AddJobHandleForProducer (inputDeps);
            m_MoveActionsGroup.AddDependency (inputDeps);
        }
        m_UniqueTypes.Clear ();
        return inputDeps;
    }
}