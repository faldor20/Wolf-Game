using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
public class SafenessComponent : MonoBehaviour

{
    // Summary:
    // increased by other prey within closest sight
    public int Safeness;
    public int Fear; //increaed by number of enemies in sight
    public int SafenessToMoveAt;
    public int MinSafenessToMoveTo;
}