using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
public class PreyMoveSystem : ComponentSystem
{
    struct Prey
    {
        public readonly int Length;
        public ComponentArray<GroupComponent> GroupComponent;
        public ComponentArray<MoveComponent> MoveComponent;
        public ComponentArray<Transform> Transform;
        public ComponentArray<Rigidbody> RigidBody;
        public ComponentArray<SightComponent> SightComponent;
        public ComponentArray<SafenessComponent> SafenessComponent;
    }
    struct GroupManager
    {
        Vector3 MoveDirection;
    }

    [Inject] private Prey _prey;
    [Inject] private ComponentArray<GroupmanagerComponent> _groupManager;
    // Update is called once per frame
    protected override void OnUpdate()
    {
        float deltaTime = Time.deltaTime;
        for (int i = 0; i < _prey.Length; i++)
        {
            //Each movement is calculated as a direction and then normalized.
            //That way there importance can be adjusted buy multiplying them by a fraction of 1 
            // then they can all be added together to get the desired move direction
            Vector3 preyPosition = _prey.Transform[i].position;
            Vector3 GroupMovement = Vector3.zero;
            if (_prey.SafenessComponent[i].Safeness > 10)
            {
                GroupMovement = (_groupManager[0].GroupPosition[_prey.GroupComponent[i].ID] - preyPosition).normalized;
            }

            Collider[] Enemies = Physics.OverlapSphere(preyPosition, _prey.SightComponent[i].Far, 9, QueryTriggerInteraction.Ignore);
            Vector3 wolfDirection = Vector3.zero;
            if (Enemies.Length > 0)
            {
                foreach (var enemy in Enemies)
                {
                    wolfDirection += enemy.transform.position - preyPosition;
                }
                Vector3 wolfMoveDirection = (-wolfDirection).normalized;
            }
            //We then collect all the normalized movedirections and calculate the final direction.
            Vector3 finalDriection =
                (
                    (GroupMovement * 0.2f) +
                    (wolfDirection * 0.6f) +
                    (_groupManager[i].Movedirections[_prey.GroupComponent[i].ID])
                ).normalized;
            Move(finalDriection,
                _prey.RigidBody[i],
                _prey.Transform[i],
                deltaTime,
                _prey.MoveComponent[i].TurnSpeed,
                _prey.MoveComponent[i].MoveSpeed);

        }
    }
    void Move(Vector3 direction, Rigidbody m_Rigidbody, Transform transform, float deltaTime, float MaxTurnSpeed, float MoveSpeed)
    {

        m_Rigidbody.AddForce((direction * MoveSpeed) * deltaTime);
        if (m_Rigidbody.velocity.magnitude > 0.01f)
        {
            m_Rigidbody.MoveRotation(Quaternion.LookRotation(new Vector3(m_Rigidbody.velocity.x, 0, m_Rigidbody.velocity.z)));
        }
        Quaternion wanted_rotation = Quaternion.LookRotation(new Vector3(m_Rigidbody.velocity.x, 0, m_Rigidbody.velocity.z));
        Quaternion.RotateTowards(transform.rotation, wanted_rotation, MaxTurnSpeed * deltaTime);
    }
}