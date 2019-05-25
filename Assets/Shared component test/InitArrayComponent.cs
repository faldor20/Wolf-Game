using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public struct InitArray : IComponentData
{

}
public interface IReceivingArrayData { }
public interface ISupplyingArrayData { }
public class InitArrayComponent : ComponentDataProxy<InitArray> { }