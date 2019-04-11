using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct MemberOfGroup : IComponentData

{
    public Entity Group;
}
public class MemberOfGroupComponent : ComponentDataProxy<MemberOfGroup> { }