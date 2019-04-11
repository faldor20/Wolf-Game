using System;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct Heading : IComponentData
{
    public float2 Value;

    public Heading(float2 heading)
    {
        Value = heading;
    }
}

[UnityEngine.DisallowMultipleComponent]
public class HeadingComponent : ComponentDataProxy<Heading> { }