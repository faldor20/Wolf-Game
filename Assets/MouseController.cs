using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {
public GameObject spawnObject;
public Vector3 mousePos;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButton(0))
		{
        mousePos.z = 3;
        Debug.Log(mousePos);
            Instantiate(spawnObject).transform.position = mousePos;
        }
	}
}
