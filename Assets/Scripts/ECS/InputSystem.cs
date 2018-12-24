using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class InputSystem : ComponentSystem
{

    struct Data
    {
        public readonly int Length;
        public ComponentArray<InputComponent> InputComponents;
    }

    [Inject] private Data _data;
    protected override void OnUpdate ()
    {
        float hzIn = Input.GetAxis ("Horizontal");;
        float vrIn = Input.GetAxis ("Vertical");
        for (int i = 0; i < _data.Length; i++)
        {
            _data.InputComponents[i].Horizontal = hzIn;
            _data.InputComponents[i].Vertical = vrIn;
        }
    }
}