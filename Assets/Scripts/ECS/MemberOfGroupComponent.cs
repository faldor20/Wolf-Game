using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class MemberOfGroupComponent : IComponentData

{
    public Entity Group;
}
public class MemberOfGroupComponent : ComponentDataProxy<MemberOfGroupComponent> { }