using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
public struct InputHandler : IComponentData
{
    public float Horizontal;
    public float Vertical;
}

public class InputHandlerComponent : ComponentDataProxy<InputHandler> { }