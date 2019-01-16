using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
public class SafenessUpdaterSystem : ComponentSystem
{
    struct Prey
    {
        public readonly int Length;
        public ComponentArray<Transform> Transform;
        public ComponentArray<SightComponent> SightComponent;
        public ComponentArray<SafenessComponent> SafenessComponent;
    }

    [Inject] private Prey _prey;
    LayerMask mask;
    // Update is called once per frame
    protected override void OnUpdate()
    {
        mask = LayerMask.GetMask("Entities");

        ContactFilter2D preyfilter = new ContactFilter2D();
        Collider[] NearbyPrey;
        for (int i = 0; i < _prey.Length; i++)
        { //TODO: presently this system does not take into account the groups of the entities it looks at
            //ill need to figure out if contact filters apply differently to the standard collision matrix thing in unities settings. or if that even works with trigger colliders;
            //   SphereCollider closeCollider = _prey.SightComponent[i].Close;
            NearbyPrey = Physics.OverlapSphere(_prey.Transform[i].position, _prey.SightComponent[i].Close, mask); //need to set the layermask
            int newSafeness = 0;
            for (int j = 0; j < NearbyPrey.Length; j++)
            {
                if (NearbyPrey[j].CompareTag("Predator")) newSafeness -= 3; // This amount is temporary and will probably increase to offset the relative rareness of wolfs
                else if (NearbyPrey[j].CompareTag("Prey"))
                {
                    newSafeness++;
                }
                else Debug.LogWarning($"Found this :{NearbyPrey[j].gameObject.name} object that was not predator or prey, filter should be adjusted");
            }
            _prey.SafenessComponent[i].Safeness = newSafeness;
        }
    }

}