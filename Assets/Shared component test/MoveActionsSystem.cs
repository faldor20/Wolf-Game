using System;
using System.Collections;
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

    //   public NativeHashMap<Entity, StoreableEntityData> m_storeableEntityData = new NativeHashMap<Entity, StoreableEntityData> ();

    BeginInitializationEntityCommandBufferSystem m_EntityCommandBufferSystem;
    protected override void OnCreate()
    { //This is our barrier system
        // Cache the BeginInitializationEntityCommandBufferSystem in a field, so we don't have to create it every frame
        m_EntityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();

        m_MoveActionsGroup = GetEntityQuery(new EntityQueryDesc
        {
            All = new [] { ComponentType.ReadOnly<MoveActions>(), ComponentType.ReadWrite<Translation>() },
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
    [RequireComponentTag(typeof(MoveActions))]
    struct ExecuteActions : IJobForEachWithEntity<Translation, Rotation>
    {
        public float deltaTime;
        // [ReadOnly] public NativeHashMap<Entity, StoreableEntityData> storableEntityData;
        // [ReadOnly] public MoveActions moveActions;
        [ReadOnly] public NativeArray<float> distances;
        [ReadOnly] public NativeArray<float> rotations;

        [ReadOnly] public EntityCommandBuffer CommandBuffer;
        //  public Unity.Mathematics.Random randomizer;
        //    public NativeArray<Translation> postion;
        public void Execute(Entity entity, int index, ref Translation position, ref Rotation rotation)
        {
            // randomizer = new Unity.Mathematics.Random ();
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
            position.Value += new float3(0, 0, 0.1f) * deltaTime;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="movedirecion"> A unit vector pointing in the direction of the movement to take place in world space</param>
        /// <param name="dt"> Delta time</param>
        public float3 Move(float3 currentPos, float3 moveDirection, float dt)
        {
            var newPos = currentPos + (moveDirection * dt);
            return newPos;
        }
    }

    private EntityQuery m_MoveActionsGroup;

    public static String ArrayToString(NativeArray<float> array)
    {
        String finalString = "";
        foreach (var item in array)
        {
            finalString += item.ToString() + "|";
        }
        return finalString;
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        EntityManager.GetAllUniqueSharedComponentData(m_UniqueTypes);
        //  Debug.Log (m_UniqueTypes.Count);
        //we start at one because the first one is simply the uninitialised version
        for (int i = 1; i < m_UniqueTypes.Count; i++)
        {
            //  Debug.Log ("distance: " + ArrayToString( m_UniqueTypes[i].distances));
            //  Debug.Log ("rotations: " + ArrayToString(m_UniqueTypes[i].rotations));
            m_MoveActionsGroup.SetFilter(m_UniqueTypes[i]);

            NativeArray<float> moveActionsDistances = new NativeArray<float>();
            NativeArray<float> moveActionsRotations = new NativeArray<float>();
            if (m_UniqueTypes[i].distances.Length != m_UniqueTypes[i].rotations.Length) { Debug.LogError("there must be a rotation for each direction given"); }
            if (m_UniqueTypes[i].distances.Length > 0) { moveActionsDistances = new NativeArray<float>(m_UniqueTypes[i].distances, Allocator.TempJob); }
            else { Debug.LogError("movactions distance has no data, without any actions this will fail."); }
            if (m_UniqueTypes[i].rotations.Length > 0) { moveActionsRotations = new NativeArray<float>(m_UniqueTypes[i].rotations, Allocator.TempJob); }
            else { Debug.LogError("movactions rotations has no data, without any actions this will fail."); }

            //  var tran else { Debug.LogError("movactions has no data, without any actiosn this will fail.") }slation = m_MoveActionsGroup.ToComponentDataArray<Translation> (Allocator.TempJob, out JobHandle getTranslationJobHandle);
            //    var combinedHandles = JobHandle.CombineDependencies (inputDeps, getTranslationJobHandle);
            var moveActions = m_UniqueTypes[i];
            ExecuteActions executeActionsJob = new ExecuteActions
            {
                deltaTime = Time.deltaTime,
                // storableEntityData = m_storeableEntityData,
                // postion = translation,
                //moveActions = m_UniqueTypes[i],
                rotations = moveActionsRotations,
                distances = moveActionsDistances,
                CommandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer()

            };
            JobHandle executeActionsJobHandle = executeActionsJob.Schedule(this, inputDeps);
            inputDeps = executeActionsJobHandle;

            m_EntityCommandBufferSystem.AddJobHandleForProducer(inputDeps);
            m_MoveActionsGroup.AddDependency(inputDeps);
        }
        m_UniqueTypes.Clear();

        return inputDeps;
    }
    protected override void OnStopRunning()
    {

    }
}