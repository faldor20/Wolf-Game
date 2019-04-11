using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Entities;
using UnityEngine;
public struct Group : IComponentData

{
   public int ID;
   //public Vector3 Direction;
}
public class GroupComponent : ComponentDataProxy<Group> { }