using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
[UpdateAfter(typeof(ElkSpawner))]
public class GroupManagerSystem : ComponentSystem
{
    bool preygot = false;
    struct Prey
    {
        public readonly int Length;
        public ComponentArray<GroupComponent> GroupComponent;
        public ComponentArray<Transform> Transform;
    }
    struct GroupManager
    {
        public readonly int Length;
        public ComponentArray<GroupmanagerComponent> Manager;
    }

    [Inject] GroupManager _groupManager;
    [Inject] private Prey _prey;
    // Update is called once per frame
    protected override void OnUpdate()
    {
        GroupmanagerComponent groupManager = null;
        if (_groupManager.Length != 0)
            groupManager = _groupManager.Manager[0];
        else Debug.LogError("No Group Manager component in scene");
        if (preygot == false)
        {
            GetPreyForEachGroup(groupManager);
            preygot = true;
        }
        for (int i = 0; i < groupManager.PreyTransform.Length; i++)
        {
            Vector3 groupPosition = Vector3.zero;
            for (int j = 0; j < 3; j++)
            {
                int numToCheck = Random.Range(0, groupManager.PreyTransform[i].Length);
                groupPosition += groupManager.PreyTransform[i][numToCheck].position;
            }
            Vector3 groupPositionAverage = groupPosition / 3;
            groupManager.GroupPosition[i] = groupPositionAverage;

        }
    }
    void GetPreyForEachGroup(GroupmanagerComponent groupManager)
    {
        for (int i = 0; i < _prey.Length; i++)
        {
            //This monstrosity creates fills the groupmanagers array containing all the prey for each group
            groupManager.PreyTransform[_prey.GroupComponent[i].ID][groupManager.PreyTransform[_prey.GroupComponent[i].ID].Length] = _prey.Transform[i];
        }
    }
}