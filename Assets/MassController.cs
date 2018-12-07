using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassController : MonoBehaviour
{
    List<GameObject> touching = new List<GameObject>();
    List<GameObject> close = new List<GameObject>();
    List<GameObject> medium = new List<GameObject>();
    List<GameObject> far = new List<GameObject>();
    List<GameObject>[] lists;
    Rigidbody rb;
    float lastTime = 1000;
    public float moveSpeed;
    public Transform mousePos;
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lists = new List<GameObject>[] { close, medium, far };
    }

    // Update is called once per frame
    void Update()
    {
        if (close.Count > 3)
        {
            rb.AddForce(mousePos.position - transform.position.normalized * moveSpeed);
        }
        else
        {

            int choser = Random.Range(0, 100);
            if (choser < 40)
            {
                if (touching.Count > 0)
                {
                    rb.AddForce((touching[Random.Range(0, touching.Count)].transform.position - transform.position).normalized * moveSpeed);
                    while (touching.Count > 5)
                    {
                        touching.RemoveAt(0);
                    }
                    return;
                }
                else
                {
                    choser = 51;
                }
            }
            if (choser < 70)
            {
                if (close.Count > 0)
                {
                    rb.AddForce((close[Random.Range(0, close.Count)].transform.position - transform.position).normalized * moveSpeed);
                    while (close.Count > 5)
                    {
                        close.RemoveAt(0);
                    }
                    return;
                }
                else
                {
                    choser = 71;
                }
            }

            if (choser < 90)
            {
                if (medium.Count > 0)
                {
                    rb.AddForce((medium[Random.Range(0, medium.Count)].transform.position - transform.position).normalized * moveSpeed);
                    while (medium.Count > 5)
                    {
                        medium.RemoveAt(0);
                    }
                    return;
                }
                else
                {
                    choser = 91;
                }
            }
            if (choser > 90)
            {
                if (far.Count > 0)
                {
                    rb.AddForce((far[Random.Range(0, far.Count)].transform.position - transform.position).normalized * moveSpeed);
                    while (far.Count > 5)
                    {
                        far.RemoveAt(0);
                    }
                    return;
                }
                else
                {
                    rb.AddForce(Random.insideUnitSphere);
                }
            }
            return;
        }

    }
    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    void OnCollisionEnter(Collision other)
    {
        touching.Add(other.gameObject);
    }
    public void AddObject(int ListNumber, GameObject obj)
    {
        lists[ListNumber].Add(obj);
    }

}