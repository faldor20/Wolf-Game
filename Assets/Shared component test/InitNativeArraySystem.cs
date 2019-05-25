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

public class InitNativeArraySystem : JobComponentSystem
    {
//we get each entity that has the InitNativeArrayComponent on them and then set
      /*   [BurstCompile]
        struct ExecuteActions : IJob<>
        {

        } */

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {

        return inputDeps;
    }
}