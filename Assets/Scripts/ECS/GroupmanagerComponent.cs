using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
public class GroupmanagerComponent : MonoBehaviour

{
    public Vector3[] Movedirections;
    public Vector3[] GroupPosition;
    public Transform[][] PreyTransform;
    public Vector3 GroupMoveSpeed;
}