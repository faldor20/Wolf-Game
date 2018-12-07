using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderController : MonoBehaviour
{
    public int colliderNumber;

    void OnTriggerEnter(Collider other)
    {
        GetComponentInParent<MassController>().AddObject(colliderNumber, other.gameObject);
    }
}
