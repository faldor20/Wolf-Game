using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
public class GroupSystem : ComponentSystem
{
    struct Data
    {
        public GroupID groupID;
        public Vision vision;
    }
    // Update is called once per frame
    protected override void OnUpdate()
    {
        GetEntities();
    }
}