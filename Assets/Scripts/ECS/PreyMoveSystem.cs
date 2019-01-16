 using System.Collections.Generic;
 using System.Collections;
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
         public readonly int Length;
         public ComponentArray<GroupmanagerComponent> Manager;
     }

     [Inject] GroupManager _groupManager;
     [Inject] private Prey _prey;
     LayerMask mask;
     // Update is called once per frame
     protected override void OnUpdate()
     {
         mask = LayerMask.GetMask("Entities");

         float deltaTime = Time.deltaTime;
         GroupmanagerComponent groupManager = _groupManager.Manager[0];
         for (int i = 0; i < _prey.Length; i++)
         {
             //Each movement is calculated as a direction and then normalized.
             //That way there importance can be adjusted buy multiplying them by a fraction of 1 
             // then they can all be added together to get the desired move direction
             Vector3 preyPosition = _prey.Transform[i].position;
             Vector3 GroupMovement = Vector3.zero;
             //this means that if the prey is not nearby 10 of the group members or 10 percent of the group
             if (_prey.SafenessComponent[i].Safeness < 10 || _prey.SafenessComponent[i].Safeness < groupManager.PreyTransform[_prey.GroupComponent[i].ID].Length / 10)
             {
                 GroupMovement = (groupManager.GroupPosition[_prey.GroupComponent[i].ID] - preyPosition) / 10;
                 if (GroupMovement.magnitude >= 1) GroupMovement = GroupMovement.normalized;
             }

             Collider[] Entities = Physics.OverlapSphere(preyPosition, _prey.SightComponent[i].Far, mask, QueryTriggerInteraction.Ignore);
             Vector3 wolfDirection = Vector3.zero;
             if (Entities.Length > 0)
             {
                 foreach (Collider entity in Entities)
                 {
                     if (entity.CompareTag("Predator"))
                     {
                         wolfDirection = preyPosition - entity.transform.position;
                     }
                 }
                 wolfDirection = (wolfDirection).normalized;
             }
             //We then collect all the normalized movedirections and calculate the final direction.
             Vector3 finalDriection = ((GroupMovement * 0.2f) + (wolfDirection * 0.6f) + ((groupManager.Movedirections[_prey.GroupComponent[i].ID]).normalized * 0.2f));

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

         m_Rigidbody.AddForce((direction.normalized * MoveSpeed) * deltaTime);
         if (m_Rigidbody.velocity.magnitude > 0.01f)
         {
             m_Rigidbody.MoveRotation(Quaternion.LookRotation(new Vector3(m_Rigidbody.velocity.x, 0, m_Rigidbody.velocity.z)));
         }
         Quaternion wanted_rotation = Quaternion.LookRotation(new Vector3(m_Rigidbody.velocity.x, 0, m_Rigidbody.velocity.z));
         Quaternion.RotateTowards(transform.rotation, wanted_rotation, MaxTurnSpeed * deltaTime);
     }
 }