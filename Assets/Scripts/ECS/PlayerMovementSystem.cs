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
    protected override void OnUpdate ()
    {
        float deltaTime = Time.deltaTime;
        for (int i = 0; i < _players.Length; i++)
        {
            Move (
                _players.InputComponents[i].Horizontal,
                _players.InputComponents[i].Vertical,
                _players.RigidBody[i],
                _players.Transform[i],
                deltaTime,
                _players.MoveComponent[i].TurnSpeed,
                _players.MoveComponent[i].MoveSpeed
            );
        }
    }
    void Move (float hzInput, float vrInput, Rigidbody m_Rigidbody, Transform transform, float deltaTime, float MaxTurnSpeed, float MoveSpeed)
    {
        Vector3 input = new Vector3 (hzInput, 0, vrInput);
        m_Rigidbody.AddForce ((input * MoveSpeed) * Time.deltaTime);
        if (m_Rigidbody.velocity.magnitude > 0.01f)
        {
            m_Rigidbody.MoveRotation (Quaternion.LookRotation (new Vector3 (m_Rigidbody.velocity.x, 0, m_Rigidbody.velocity.z)));
        }
        Quaternion wanted_rotation = Quaternion.LookRotation (new Vector3 (m_Rigidbody.velocity.x, 0, m_Rigidbody.velocity.z));
        Quaternion.RotateTowards (transform.rotation, wanted_rotation, MaxTurnSpeed * Time.deltaTime);
    }
}