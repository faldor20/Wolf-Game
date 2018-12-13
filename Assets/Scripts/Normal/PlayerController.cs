using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{

    Rigidbody m_Rigidbody;

    public float speed = 1;
    public float MaxTurnSpeed = 1;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    void Move()
    {
        float hzInput = Input.GetAxisRaw("Horizontal");
        float vrInput = Input.GetAxisRaw("Vertical");
        Vector3 input = new Vector3(hzInput, 0, vrInput);
        m_Rigidbody.AddForce((input * speed) * Time.deltaTime);
        if (m_Rigidbody.velocity.magnitude > 0.01f)
        {
            m_Rigidbody.MoveRotation(Quaternion.LookRotation(new Vector3(m_Rigidbody.velocity.x, 0, m_Rigidbody.velocity.z)));
        }
        Quaternion wanted_rotation = Quaternion.LookRotation(new Vector3(m_Rigidbody.velocity.x, 0, m_Rigidbody.velocity.z));
        Quaternion.RotateTowards(transform.rotation, wanted_rotation, MaxTurnSpeed * Time.deltaTime);
    }
}