using System;
using System.Collections.Generic;
using Samples.Common;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class BoidMoveSystem : JobComponentSystem
{

    private ComponentGroup m_BoidGroup;
    private ComponentGroup m_TargetGroup;
    private ComponentGroup m_ObstacleGroup;
    private ComponentGroup m_GoupManager;

    private List<MainBoid> m_UniqueTypes = new List<MainBoid>(10);
    private List<PrevCells> m_PrevCells = new List<PrevCells>();

    struct PrevCells
    {
        public NativeHashMap<float3, int> hashMap;

        public NativeArray<Position> copyTargetPositions;
        public NativeArray<Position> copyObstaclePositions;

        public NativeArray<Heading> boidHeadings;
        public NativeArray<Position> boidPositions;

        public NativeArray<BoidAction> orderedBoidActions;
        public NativeArray<NonBoidAction> orderedNonBoidActions;
    }

    [RequireComponentTag(typeof(MainBoid))]
    struct HashPositions : IJobProcessComponentDataWithEntity<Position>
    {
        [WriteOnly] public NativeHashMap<float3, int>.Concurrent hashMap;

        public void Execute(Entity entity, int index, [ReadOnly] ref Position position)
        {
            hashMap.TryAdd(position.Value, index);
        }
    }

    [RequireComponentTag(typeof(MainBoid))]
    struct Steer : IJobProcessComponentData<Position, Heading>
    {
        abstract class BoidActionFunc
        {
            public BoidActionType ActionID;
            public float2 DoAction(List<int> boidsForActions, float2 currentPosition, NativeArray<Position> boidPositions, float weight)
            {
                float2 result = float2.zero;
                foreach (var boidIndex in boidsForActions)
                {
                   result= SpecificAction(currentPosition, boidPositions[boidIndex].Value.ToFloat2(), result);
                }
                //TODO: check if normalizing this is actually necissary
                return result = math.normalizesafe(result / boidsForActions.Count) * weight;
            }
            public float2 DoAction(List<int> boidsForActions, float2 currentPosition, NativeArray<Heading> boidHeadings, float weight)
            {
                float2 result = float2.zero;
                foreach (var boidIndex in boidsForActions)
                {
                  result=  SpecificAction(currentPosition, boidHeadings[boidIndex].Value, result);
                }
                //TODO: check if normalizing this is actually necissary
                return result = math.normalizesafe(result / boidsForActions.Count) * weight;
            }
            public abstract float2 SpecificAction(float2 currentPos, float2 boidPosOrHeading, float2 previousresult);
        }

        class SeperationAction : BoidActionFunc
        {
            public SeperationAction() { ActionID = BoidActionType.Seperation; }

            public override float2 SpecificAction(float2 currentPos, float2 boidPos, float2 previousresult)
            {
                return previousresult += math.normalizesafe(currentPos - boidPos);
            }
        }

        class CohesionAction : BoidActionFunc
        {
            public CohesionAction() { ActionID = BoidActionType.Cohesion; }

            public override float2 SpecificAction(float2 currentPos, float2 boidPos, float2 previousresult)
            {
                return previousresult += math.normalizesafe(boidPos - currentPos);
            }
        }

        class AlignmentAction : BoidActionFunc
        {
            public AlignmentAction() { ActionID = BoidActionType.Alignment; }

            public override float2 SpecificAction(float2 currentPos, float2 boidPos, float2 previousresult)
            {
                return previousresult += math.normalizesafe(boidPos - currentPos);
            }
        }

        //[ReadOnly] public MainBoid boidConfig;
        [ReadOnly] public NativeArray<BoidAction> orderedBoidActions;
        [ReadOnly] public NativeArray<NonBoidAction> orderedNonBoidActions;
        [ReadOnly] public NativeArray<Position> targetPositions;
        [ReadOnly] public NativeArray<Position> obstaclePositions;
        [ReadOnly] public NativeArray<Position> boidPositions;
        [ReadOnly] public NativeArray<Heading> boidHeadings;
        [ReadOnly] public NativeHashMap<float3, int> boidIndexs;
        // [ReadOnly] public BoidActionFunc[] boidActionFunctions;
        public float dt;

        public void Execute([ReadOnly] ref Position position, ref Heading heading)
        {
            BoidActionFunc[] boidActionFunctions = { new SeperationAction(), new CohesionAction(), new AlignmentAction() };
            int thisBoidIndex = boidIndexs[position.Value];
            float2 forward = heading.Value;
            float2 currentPosition = position.Value.ToFloat2();

            //This is a list of all boids needing to be checked for each action
            List<int>[] boidsForActions = CheckBoidCollision2D(currentPosition, forward, boidPositions, orderedBoidActions);
            float2 headingChanges;
            //Here we do all the actions that this boid does iterating over each action in the list and running a function asociated with its actionType enum.
            float2[] boidActionResults = new float2[orderedBoidActions.Length + orderedNonBoidActions.Length];
            for (int i = 0; i < orderedBoidActions.Length; i++)
            {
                foreach (var boidAction in boidActionFunctions)
                {
                    if (orderedBoidActions[i].actionType == boidAction.ActionID)
                    {
                        if (boidAction.ActionID == BoidActionType.Alignment)
                        {
                            boidActionResults[i] = boidAction.DoAction(boidsForActions[i], currentPosition, boidHeadings, orderedBoidActions[i].weight);
                        }
                        else
                        {
                            boidActionResults[i] = boidAction.DoAction(boidsForActions[i], currentPosition, boidPositions, orderedBoidActions[i].weight);
                        }
                        continue;

                    }
                }

                #region OldFunctions(keeping incase i want to change functionallity on the fly)
                /* 
                if (orderedBoidActions[i].actionType == BoidActionType.Alignment)
                {
                    float2 alignmentResult = float2.zero;

                    foreach (var boidIndex in boidsForActions[i])
                    {
                        alignmentResult += boidHeadings[boidIndex].Value;
                    }
                    //TODO: check if normalizing this is actually necissary
                    alignmentResult = math.normalizesafe(alignmentResult / boidsForActions[i].Count) * orderedBoidActions[i].weight;
                }
                else
                if (orderedBoidActions[i].actionType == BoidActionType.Cohesion)
                {
                    float2 cohesionResult = float2.zero;
                    foreach (var boidIndex in boidsForActions[i])
                    {
                        cohesionResult += math.normalizesafe(boidPositions[boidIndex].Value.ToFloat2() - currentPosition);
                    }
                    //TODO: check if normalizing this is actually necissary
                    cohesionResult = math.normalizesafe(cohesionResult / boidsForActions[i].Count) * orderedBoidActions[i].weight;
                }
                else if (orderedBoidActions[i].actionType == BoidActionType.Seperation)
                {
                    float2 seperationResult = float2.zero;
                    foreach (var boidIndex in boidsForActions[i])
                    {
                        seperationResult += math.normalizesafe(currentPosition - boidPositions[boidIndex].Value.ToFloat2());
                    }
                    //TODO: check if normalizing this is actually necissary
                    seperationResult = math.normalizesafe(seperationResult / boidsForActions[i].Count) * orderedBoidActions[i].weight;
                }*/
                #endregion
                if (orderedNonBoidActions[i].actionType == NonBoidActionType.Fleeing)
                {

                    float2 fleeingResult = float2.zero;
                    foreach (var obstaclePosition in obstaclePositions)
                    {
                        float2 obstaclePos = obstaclePosition.Value.ToFloat2();
                        fleeingResult += math.normalizesafe(currentPosition - obstaclePos) * (1 / math.distance(currentPosition, obstaclePos));
                    }
                    boidActionResults[orderedBoidActions.Length + 1] = math.normalizesafe(fleeingResult / boidsForActions[i].Count) * orderedBoidActions[i].weight;
                }
                else if (orderedNonBoidActions[i].actionType == NonBoidActionType.Targeting)
                {

                    float2 targetingResult = float2.zero;
                    foreach (var obstaclePosition in obstaclePositions)
                    {
                        float2 obstaclePos = obstaclePosition.Value.ToFloat2();
                        targetingResult += math.normalizesafe(obstaclePos - currentPosition) * (1 / math.distance(currentPosition, obstaclePos));
                    }
                    boidActionResults[orderedBoidActions.Length + 1] = math.normalizesafe(targetingResult / boidsForActions[i].Count) * orderedBoidActions[i].weight;
                }
            }
            float2 compiledHeadings = float2.zero;
            foreach (var boidActionResult in boidActionResults)
            {
                compiledHeadings += boidActionResult;
            }
            float2 targetForward = math.normalizesafe(compiledHeadings); //math.select(normalHeading, avoidObstacleHeading,   < 0);
            float2 nextHeading = math.normalizesafe(forward + dt * (targetForward - forward));

            heading = new Heading { Value = nextHeading };
        }
        
        
        public List<int>[] CheckBoidCollision2D(float2 centerPosition, float2 heading, NativeArray<Position> BoidPositions, NativeArray<BoidAction> orderedActions)
        {
            List<int>[] nearbyBoidIndexsForEachAction = new List<int>[orderedActions.Length];
            for (int i = 0; i < nearbyBoidIndexsForEachAction.Length; i++)
            {
                nearbyBoidIndexsForEachAction[i] = new List<int>();
            }
            for (int i = 0; i < BoidPositions.Length; i++)
            {
                float Distance = math.distance(centerPosition, BoidPositions[i].Value.ToFloat2());
                float angle = Vector2.Angle(heading, BoidPositions[i].Value.ToFloat2());

                for (int j = 0; j < orderedActions.Length; j++)
                {
                    //Because the actions are ordered we only need to check the list untill we find one that is notwithin the range.
                    if (Distance > orderedActions[j].range) break;

                    float viewAngle = orderedActions[j].viewangle;

                    if (viewAngle == 0 || viewAngle == 360 || angle < viewAngle / 2)
                    {
                        //    Debug.Log($"Index:{j} Max:{orderedActions.Length-1}");
                        nearbyBoidIndexsForEachAction[j].Add(i);
                    }
                }
            }
            return nearbyBoidIndexsForEachAction;
        }
    }

    float2 Alignment2D(float2[] nearbyheadings)
    {
        float2 NewHeading = float2.zero;
        foreach (float2 heading in nearbyheadings)
        {
            NewHeading += heading;
        }
        NewHeading /= nearbyheadings.Length;
        return NewHeading;
    }

    float2 Alignment2D(List<float2> nearbyheadings)
    {
        float2 NewHeading = float2.zero;
        foreach (float2 heading in nearbyheadings)
        {
            NewHeading += heading;
        }
        NewHeading /= nearbyheadings.Count;
        return NewHeading;
    }

    protected override void OnStopRunning()
    {
        for (int i = 0; i < m_PrevCells.Count; i++)
        {
            m_PrevCells[i].hashMap.Dispose();

            m_PrevCells[i].copyTargetPositions.Dispose();
            m_PrevCells[i].copyObstaclePositions.Dispose();

            m_PrevCells[i].boidHeadings.Dispose();
            m_PrevCells[i].boidPositions.Dispose();

            m_PrevCells[i].orderedBoidActions.Dispose();
            m_PrevCells[i].orderedNonBoidActions.Dispose();
        }
        m_PrevCells.Clear();
    }
    public class BoidActionSort : IComparer<BoidAction>
    {
        public int Compare(BoidAction a, BoidAction b)
        {
            if (a.range > b.range) return 1;
            else return -1;

        }
    }
    public class NonBoidActionSort : IComparer<NonBoidAction>
    {
        public int Compare(NonBoidAction a, NonBoidAction b)
        {
            if (a.range > b.range) return 1;
            else return -1;

        }
    }
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        BoidActionSort boidActionsSort = new BoidActionSort();
        NonBoidActionSort nonBoidActionSort = new NonBoidActionSort();
        EntityManager.GetAllUniqueSharedComponentData(m_UniqueTypes);

        int obstacleCount = m_ObstacleGroup.CalculateLength();
        int targetCount = m_TargetGroup.CalculateLength();

        // Ingore typeIndex 0, can't use the default for anything meaningful.
        for (int typeIndex = 1; typeIndex < m_UniqueTypes.Count; typeIndex++)
        {
            MainBoid uniqueBoidConfig = m_UniqueTypes[typeIndex];
            m_BoidGroup.SetFilter(uniqueBoidConfig);

            int boidCount = m_BoidGroup.CalculateLength();
            //some of this can be cached from last time to reduce component data calls.
            int cacheIndex = typeIndex - 1;
            NativeArray<JobHandle> initializationJobHandles = new NativeArray<JobHandle>(5, Allocator.Temp);

            NativeArray<Heading> boidHeadings = m_BoidGroup.ToComponentDataArray<Heading>(Allocator.TempJob, out JobHandle initialCellAlignmentJobHandle);
            //TODO: make this into a 2d array so that the 2d positions doesnt have to be calculated all the time.
            NativeArray<Position> boidPositions = m_BoidGroup.ToComponentDataArray<Position>(Allocator.TempJob, out JobHandle initialCellSeparationJobHandle);
            NativeArray<Position> copyTargetPositions = m_TargetGroup.ToComponentDataArray<Position>(Allocator.TempJob, out JobHandle copyTargetPositionsJobHandle);
            NativeArray<Position> copyObstaclePositions = m_ObstacleGroup.ToComponentDataArray<Position>(Allocator.TempJob, out JobHandle copyObstaclePositionsJobHandle);
            initializationJobHandles[0] = initialCellAlignmentJobHandle;
            initializationJobHandles[1] = initialCellSeparationJobHandle;
            initializationJobHandles[2] = copyTargetPositionsJobHandle;
            initializationJobHandles[3] = copyObstaclePositionsJobHandle;

            NativeArray<BoidAction> orderedBoidActions = new NativeArray<BoidAction>(uniqueBoidConfig.boidActions.Length, Allocator.TempJob);
            orderedBoidActions.CopyFrom(uniqueBoidConfig.boidActions);

            NativeArray<NonBoidAction> orderedNonBoidActions = new NativeArray<NonBoidAction>(uniqueBoidConfig.boidActions.Length, Allocator.TempJob);
            orderedBoidActions.CopyFrom(uniqueBoidConfig.boidActions);

            orderedNonBoidActions.Sort(nonBoidActionSort);
            orderedBoidActions.Sort(boidActionsSort);

            var hashMap = new NativeHashMap<float3, int>(boidCount, Allocator.TempJob);

            var hashPositionsJob = new HashPositions
            {
                hashMap = hashMap.ToConcurrent()
            };
            var hashPositionsJobHandle = hashPositionsJob.ScheduleGroup(m_BoidGroup, inputDeps);

            initializationJobHandles[4] = hashPositionsJobHandle;

            var nextCells = new PrevCells
            {
                hashMap = hashMap,

                boidHeadings = boidHeadings,
                boidPositions = boidPositions,

                copyObstaclePositions = copyObstaclePositions,
                copyTargetPositions = copyTargetPositions,

                orderedBoidActions = orderedBoidActions,
                orderedNonBoidActions = orderedNonBoidActions,

            };
            if (cacheIndex > (m_PrevCells.Count - 1))
            {
                m_PrevCells.Add(nextCells);
            }
            else
            {
                m_PrevCells[cacheIndex].hashMap.Dispose();

                m_PrevCells[cacheIndex].copyTargetPositions.Dispose();
                m_PrevCells[cacheIndex].copyObstaclePositions.Dispose();

                m_PrevCells[cacheIndex].boidHeadings.Dispose();
                m_PrevCells[cacheIndex].boidPositions.Dispose();

                m_PrevCells[cacheIndex].orderedBoidActions.Dispose();
                m_PrevCells[cacheIndex].orderedNonBoidActions.Dispose();
            }
            m_PrevCells[cacheIndex] = nextCells;

            JobHandle initialCellBarrierJobHandle = JobHandle.CombineDependencies(initialCellAlignmentJobHandle, initialCellSeparationJobHandle);
            JobHandle copyTargetObstacleBarrierJobHandle = JobHandle.CombineDependencies(initializationJobHandles);

            Steer steerJob = new Steer
            {
                //                boidActionFunctions = boidActionFunctions,
                //boidConfig = uniqueBoidConfig,
                boidIndexs = hashMap,
                boidHeadings = boidHeadings,
                boidPositions = boidPositions,
                orderedBoidActions = orderedBoidActions,
                orderedNonBoidActions = orderedNonBoidActions,
                targetPositions = copyTargetPositions,
                obstaclePositions = copyObstaclePositions,
                dt = Time.deltaTime
            };
            JobHandle steerJobHandle = steerJob.ScheduleGroup(m_BoidGroup, copyTargetObstacleBarrierJobHandle);

            inputDeps = steerJobHandle;
            m_BoidGroup.AddDependency(inputDeps);
        }
        m_UniqueTypes.Clear();

        return inputDeps;
    }

    protected override void OnCreateManager()
    {
        m_BoidGroup = GetComponentGroup(
            ComponentType.ReadOnly(typeof(MainBoid)),
            ComponentType.ReadOnly(typeof(Position)),
            typeof(Heading));
        m_TargetGroup = GetComponentGroup(
            ComponentType.ReadOnly(typeof(BoidTarget)),
            ComponentType.ReadOnly(typeof(Position)));
        m_ObstacleGroup = GetComponentGroup(
            ComponentType.ReadOnly(typeof(BoidObstacle)),
            ComponentType.ReadOnly(typeof(Position)));

    }

}
public static class float3extension
{
    public static float2 ToFloat2(this float3 f)
    {
        return new float2(f.x, f.y);
    }

}

public struct MyFloat2
{
    public float x;
    public float y;
    MyFloat2(float X, float Y)
    {
        x = X;
        y = Y;
    }
    public static implicit operator MyFloat2(float3 f)
    {
        return new MyFloat2(f.x, f.y);
    }
    public static implicit operator float2(MyFloat2 f)
    {
        return new float2(f.x, f.y);
    }
}