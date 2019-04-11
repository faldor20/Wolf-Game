using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
//using System;
//using UnityEngine;
public class RandomMoveSpeedSystem : JobComponentSystem
{
    [BurstCompile]
    struct RandomizeMoveSpeed : IJobForEach<RandomMoveSpeed, MoveSpeed>
    {
        public void Execute([ReadOnly] ref RandomMoveSpeed randomMoveSpeed, ref MoveSpeed moveSpeed)
        {
            if (moveSpeed.speed == 0)
            { //To make sure movespeed is never set to zero by accident
                while (moveSpeed.speed == 0)
                {
                    moveSpeed.speed = UnityEngine.Random.Range(randomMoveSpeed.min, randomMoveSpeed.max);
                }

            }
            else
            {
                if (randomMoveSpeed.varianceWhileMoving != 0)
                {
                    //TODO: impliment a variance function.
                }
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var randomizeMoveSpeedJob = new RandomizeMoveSpeed();
        var randomizeMoveSpeedJobHandle = randomizeMoveSpeedJob.Schedule(this, inputDeps);

        return randomizeMoveSpeedJobHandle;
    }
}