/* using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class PlayerMovementSystem : JobComponentSystem
{

    private EntityQuery m_players;
    [BurstCompile]
    struct PlayerMovementJob : IJobForEach<Rotation, ECSPhysics, Move, InputHandler>
    {
        public float deltaTime;
        public void Execute([ReadOnly] ref Rotation rotation, ref ECSPhysics physics, [ReadOnly] ref Move moveComponent, [ReadOnly] ref InputHandler inputComponent)
        {
            Vector3 input = new float3(inputComponent.Horizontal, 0, inputComponent.Vertical);
            Move(
                input,
                physics,
                rotation,
                deltaTime,
                moveComponent.TurnSpeed,
                moveComponent.MoveSpeed
            );
        }
    }
    //[Inject] private Players _players;
    protected override JobHandle OnUpdate(JobHandle inputDependancy)
    {
        var job = new PlayerMovementJob()
        {
            deltaTime = Time.deltaTime
        };
        return job.Schedule(this, inputDependancy);

    }
    void Move(float3 direction, ECSPhysics physics, Rotation rotation, float deltaTime, float MaxTurnSpeed, float MoveSpeed)
    {

        physics.velocity += ((direction * MoveSpeed) * deltaTime);
        if (magnitude(physics.velocity) > 0.01f)
        {
            // physics.MoveRotation(Quaternion.LookRotation(new Vector3(physics.velocity.x, 0, physics.velocity.z)));
        }
        Quaternion wanted_rotation = Quaternion.LookRotation(new Vector3(physics.velocity.x, 0, physics.velocity.z));
        Quaternion.RotateTowards(rotation.Value, wanted_rotation, MaxTurnSpeed * deltaTime);
    }
    float magnitude(float3 vec)
    {
        var outVec = math.sqrt(math.pow(vec.x, 2) + math.pow(vec.z, 2) + math.pow(vec.y, 2));
        return outVec;
    }
} */