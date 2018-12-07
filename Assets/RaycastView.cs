using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastView : MonoBehaviour {
public int castNumber;
public List<GameObject> close = new List<GameObject>();
public float moveSpeed;
Rigidbody rb;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < castNumber; i++)
		{RaycastHit hit;
            var cast = Physics.Raycast(transform.position, new Vector3(Mathf.Cos(i), 0, Mathf.Sin(i)), out hit,4);
            if(cast) close.Add(hit.collider.gameObject);
        }
       if (close.Count>0)
	   {
		 rb.AddForce((close[Random.Range(0, close.Count)].transform.position - transform.position).normalized * moveSpeed);   
	   }
	   
    }
}
