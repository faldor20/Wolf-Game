/* using System.Collections;
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

        //in the final result this will just move along in the path defined by group movement and be updated every few seconds to correct
        for (int i = 0; i < groupManager.PreyTransform.Length; i++)
        {
            Vector3 groupPosition = Vector3.zero;
            int numberOfPreyInGroup = groupManager.PreyTransform[i].Length;
            for (int j = 0; j < numberOfPreyInGroup; j++)
            {
                groupPosition += groupManager.PreyTransform[i][j].position;
            }
            Vector3 groupPositionAverage = groupPosition / numberOfPreyInGroup;
            groupManager.GroupPosition[i] = groupPositionAverage;

        }
    }
    void GetPreyForEachGroup(GroupmanagerComponent groupManager)
    {

    }
} */