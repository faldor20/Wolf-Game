using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElkSpawner : MonoBehaviour
{

    public int numberOfGroups = 3;
    public int maxGroupSize = 20;
    public int minGroupSize = 5;
    public GameObject elkPrefab;
    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < numberOfGroups; i++)
        {
            int groupSize = Random.Range(minGroupSize, maxGroupSize); //This should really follow a nroaml distribution but for now uitll just be random
            CreateGroup(groupSize, new Vector3(0, 0, i * 20), i);
        }
    }
    void CreateGroup(int size, Vector3 location, int groupID)
    {
        int columnLength = Mathf.CeilToInt(Mathf.Sqrt(size));
        int spawned = 0;
        Vector3 startPoint = new Vector3(location.x - (columnLength / 2), location.y, location.z - (columnLength / 2));
        for (int i = 0; i < columnLength; i++)
        {
            for (int j = 0; j < columnLength; j++)
            {
                if (spawned >= size) break;
                GameObject elk = Instantiate(elkPrefab, startPoint + new Vector3(i, 0, j), Quaternion.identity);
                elk.GetComponent<GroupComponent>().ID = groupID;
            }
            if (spawned >= size) break;
        }
    }

}