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

    public List<MoveActions> m_UniqueTypes = new List<MoveActions>(10);

    public NativeHashMap<Entity, StoreableEntityData> m_storeableEntityData = new NativeHashMap<Entity, StoreableEntityData>();

    public struct StoreableEntityData
    {
        public float waitTimer;
        public int stepsCompleted;
        public int distanceRemaining;
    }

    [BurstCompile]
    [RequireComponentTag(typeof(MoveActions))]
    struct ExecuteActions : IJobForEachWithEntity<Translation, Rotation>
    {
        public float deltaTime;
        public NativeHashMap<Entity, StoreableEntityData> storableEntityData;
        [ReadOnly] public MoveActions moveActions;
        public NativeArray<Translation> postion;
        public void Execute(Entity entity, int index, ref Translation position, ref Rotation rotation)
        {
            var storedData = storableEntityData[entity];
            if (storedData.waitTimer <= 0 && storedData.distanceRemaining <= 0)
            {
                if (moveActions.directions[storableEntityData[entity].stepsCompleted])

            }
            if (storedData.distanceRemaining > 0)
            {
                var rot = rotation.Value;
                var rotat = math.quaternion(rot.value);
                quaternion.AxisAngle
                rot.value.RotateY(-90);
            }
        }
    }

    //movedirection must be a vector of magnitude 1
    /// <summary>
    /// 
    /// </summary>
    /// <param name="movedirecion"> A unit vector pinting in the direction of the movement to take place in world space</param>
    /// <param name="dt"> Delta time</param>
    void move(float3 movedirecion, float dt)
    {
        Translation = Translation + (movedirection * dt)
    }
    private EntityQuery m_MoveActionsGroup;

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        EntityManager.GetAllUniqueSharedComponentData(m_UniqueTypes);
        Debug.Log(m_UniqueTypes.Count);
        for (int i = 0; i < m_UniqueTypes.Count; i++)
        {
            m_MoveActionsGroup.SetFilter(m_UniqueTypes[i]);

            var translation = m_MoveActionsGroup.ToComponentDataArray<Translation>(Allocator.TempJob, out JobHandle getTranslationJobHandle);
            var combinedHandles = JobHandle.CombineDependencies(inputDeps, getTranslationJobHandle);
            var moveActions = m_UniqueTypes[i];
            ExecuteActions executeActionsJob = new ExecuteActions
            {
                deltaTime = Time.deltaTime,
                storableEntityData = m_storeableEntityData,
                postion = translation,
                moveActions = m_UniqueTypes[i]

            };
            JobHandle executeActionsJobHandle = executeActionsJob.Schedule(getTranslationJobHandle);
            inputDeps = getTranslationJobHandle;
            m_MoveActionsGroup.AddDependency(inputDeps);

        }
        m_UniqueTypes.Clear();
        return inputDeps;
    }

    protected override void OnCreate()
    {
        m_MoveActionsGroup = GetEntityQuery(new EntityQueryDesc
        {
            All = new [] { ComponentType.ReadOnly<MoveActions>(), ComponentType.ReadWrite<Translation>() },
                Options = EntityQueryOptions.Default
        });
    }
}