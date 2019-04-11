using System;
using Unity.Entities;

/// <summary>
///This allows a random movement speed to be set initially then changed during runtime if desired.
/// output must not be zero
/// </summary>
/// 
//TODO:this could be optimized by making it shared componant data, because most boids should have the same values.
[Serializable]
public struct RandomMoveSpeed : IComponentData
{
    //min must be above zero.
    public float min;
    public float max;
    public ByteBool changeOverTime;
    public float changeSpeed;
    //This is the range from the initial random value taht speed can vary by
    public float varianceWhileMoving;
}

[UnityEngine.DisallowMultipleComponent]
public class RandomMoveSpeedComponent : ComponentDataProxy<RandomMoveSpeed> { }