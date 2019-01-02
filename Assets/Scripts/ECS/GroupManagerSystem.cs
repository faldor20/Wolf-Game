using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
public class GroupManagerSystem : ComponentSystem
{
    struct Prey
    {
        public readonly int Length;
        public ComponentArray<GroupComponent> GroupComponent;
        public ComponentArray<Transform> Transform;
    }

    [Inject] private ComponentArray<GroupmanagerComponent> _groupManager;
    [Inject] private Prey _prey;
    // Update is called once per frame
    protected override void OnUpdate()
    {
        for (int i = 0; i < _groupManager[0].PreyTransform.Length; i++)
        {
            Vector3 groupPosition = Vector3.zero;
            for (int j = 0; j < 3; j++)
            {
                int numToCheck = Random.Range(0, _groupManager[0].PreyTransform[i].Length);
                groupPosition += _groupManager[0].PreyTransform[i][numToCheck].position;
            }
            Vector3 groupPositionAverage = groupPosition / 3;
            _groupManager[0].GroupPosition[i] = groupPositionAverage;

        }
    }
    void GetPreyForEachGroup()
    {
        for (int i = 0; i < _prey.Length; i++)
        {
            //This monstrosity creates fills the groupmanagers array containing all the prey for each group
            _groupManager[0].PreyTransform[_prey.GroupComponent[i].ID][_groupManager[0].PreyTransform[_prey.GroupComponent[i].ID].Length - 1] = _prey.Transform[i];
        }
    }
}