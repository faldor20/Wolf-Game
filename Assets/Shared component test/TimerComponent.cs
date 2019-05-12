using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
[Serializable]
public struct Timer : ISharedComponentData
{
public float timeLeft;
}


public class TimerComponent : SharedComponentDataProxy<Timer> { }