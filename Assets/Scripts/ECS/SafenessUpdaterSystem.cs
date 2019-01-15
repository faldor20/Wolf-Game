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

    // Update is called once per frame
    protected override void OnUpdate()
    {
        ContactFilter2D preyfilter = new ContactFilter2D();
        Collider[] NearbyPrey;
        for (int i = 0; i < _prey.Length; i++)
        { //TODO: presently this system does not take into account the groups of the entities it looks at
            //ill need to figure out if contact filters apply differently to the standard collision matrix thing in unities settings. or if that even works with trigger colliders;
            //   SphereCollider closeCollider = _prey.SightComponent[i].Close;
            NearbyPrey = Physics.OverlapSphere(_prey.Transform[i].position, _prey.SightComponent[i].Close, 9); //need to set the layermask
            for (int j = 0; j < NearbyPrey.Length; j++)
            {
                if (NearbyPrey[j].CompareTag("Predator")) _prey.SafenessComponent[i].Safeness -= 3; // This amount is temporary and will probably increase to offset the relative rareness of wolfs
                else if (NearbyPrey[j].CompareTag("Prey"))
                {
                    _prey.SafenessComponent[i].Safeness++;
                }
                else Debug.LogWarning("Found an object that was not predator or prey, filter should be adjusted");
            }
        }
    }

}