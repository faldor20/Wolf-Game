using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
[Serializable]
public struct GroupManager : IComponentData

{
    public Vector3[] Movedirections;
    public Vector3[] GroupPosition;
    public Transform[][] PreyTransform;
    public Vector3 GroupMoveSpeed;
}
public class GroupmManagerComponent : ComponentDataProxy<GroupManager> { }