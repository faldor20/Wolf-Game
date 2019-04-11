using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class ECSPhysicsSystem : JobComponentSystem
{
    //  private EntityQuery m_players;
    [BurstCompile]
    struct PhysicsUpdateJob : IJobForEach<ECSPhysics, Translation>
    {
        public float deltaTime;
        public void Execute(ref ECSPhysics physics, ref Translation translation)
        {
            translation.Value += physics.velocity * Time.deltaTime;
            translation.Value = math.lerp(physics.velocity, float3.zero, (physics.drag * deltaTime) / physics.velocity);
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDependancy)
    {

        var job = new PhysicsUpdateJob()
        {
            deltaTime = Time.deltaTime
        };
        return job.Schedule(this, inputDependancy);

    }
}