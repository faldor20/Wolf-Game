using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

public class RandomInitialHeadingSystem : JobComponentSystem
{
    // BeginInitializationEntityCommandBufferSystem is used to create a command buffer which will then be played back
    // when that barrier system executes.
    // Though the instantiation command is recorded in the SpawnJob, it's not actually processed (or "played back")
    // until the corresponding EntityCommandBufferSystem is updated. To ensure that the transform system has a chance
    // to run on the newly-spawned entities before they're rendered for the first time, the HelloSpawnerSystem
    // will use the BeginSimulationEntityCommandBufferSystem to play back its commands. This introduces a one-frame lag
    // between recording the commands and instantiating the entities, but in practice this is usually not noticeable.
    BeginInitializationEntityCommandBufferSystem m_EntityCommandBufferSystem;

    struct SetInitialHeadingJob : IJobForEachWithEntity<RandomInitialHeading, Heading>
    {
        [ReadOnly] public Unity.Mathematics.Random Random;
        public EntityCommandBuffer CommandBuffer;

        //Random initial heading must be set to readonly becuase it is the subject of the command buffer
        public void Execute(Entity entity, int index, [ReadOnly] ref RandomInitialHeading randomInitialHeading, ref Heading heading)
        {
            heading = new Heading
            {
                Value = Random.NextFloat2Direction()
            };

            CommandBuffer.RemoveComponent<RandomInitialHeading>(entity);

        }
    }
    protected override void OnCreate()
    { //This is our barrier system
        // Cache the BeginInitializationEntityCommandBufferSystem in a field, so we don't have to create it every frame
        m_EntityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {

        var job = new SetInitialHeadingJob
        {
            CommandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer(),
            Random = new Random(0xabcdef)
        };
        //this needs ti be single or else it will error out
        var handle = job.ScheduleSingle(this, inputDeps);
        // SpawnJob runs in parallel with no sync point until the barrier system executes.
        // When the barrier system executes we want to complete the SpawnJob and then play back the commands (Creating the entities and placing them).
        // We need to tell the barrier system which job it needs to complete before it can play back the commands.
        m_EntityCommandBufferSystem.AddJobHandleForProducer(handle);
        return handle;

    }
}