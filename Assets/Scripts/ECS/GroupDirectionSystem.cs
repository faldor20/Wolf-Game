using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
public class GroupDirectionSystem : ComponentSystem
{

    public ComponentArray<GroupmanagerComponent> GroupManager;

    [Inject] public ComponentArray<GroupComponent> Groups;
    // Update is called once per frame
    protected override void OnUpdate()
    {
        for (int i = 0; i < Groups.Length; i++)
        {
            //  Groupmanager Groups[i].ID
        }
    }
}