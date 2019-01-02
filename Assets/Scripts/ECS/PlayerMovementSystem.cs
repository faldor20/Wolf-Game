using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
public class PlayerMovementSystem : ComponentSystem
{
    struct Players //This essentially filters the entities and returns only those containing this list of components
    {
        public readonly int Length;
        public ComponentArray<Transform> Transform;
        public ComponentArray<Rigidbody> RigidBody;
        public ComponentArray<MoveComponent> MoveComponent;
        public ComponentArray<InputComponent> InputComponents;
    }

    [Inject] private Players _players;
    protected override void OnUpdate()
    {
        float deltaTime = Time.deltaTime;
        for (int i = 0; i < _players.Length; i++)
        {
            Vector3 input = new Vector3(_players.InputComponents[i].Horizontal, 0, _players.InputComponents[i].Vertical);
            Move(
                input,
                _players.RigidBody[i],
                _players.Transform[i],
                deltaTime,
                _players.MoveComponent[i].TurnSpeed,
                _players.MoveComponent[i].MoveSpeed
            );
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