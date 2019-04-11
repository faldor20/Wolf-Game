using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class InputSystem : JobComponentSystem
{

    /*   struct Data
      {
          public readonly int Length;
          public ComponentArray<InputComponent> InputComponents;
      } */

    //  [Inject] private Data _data;
    [BurstCompile]
    struct InputSystemJob : IJobForEach<InputHandler>
    {
        public float hzIn;
        public float vrIn;
        public void Execute([WriteOnly] ref InputHandler inputComponent)
        {
            inputComponent.Horizontal = hzIn;
            inputComponent.Vertical = vrIn;

        }
    }
    protected override JobHandle OnUpdate(JobHandle inputDependancy)
    {
        float horizontalIn = Input.GetAxis("Horizontal");
        float verticalIn = Input.GetAxis("Vertical");

        var job = new InputSystemJob()
        {
            hzIn = horizontalIn,
            vrIn = verticalIn

        };
        return job.Schedule(this, inputDependancy);
    }

}