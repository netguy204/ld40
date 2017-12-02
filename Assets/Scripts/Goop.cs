using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goop : MonoBehaviour {
    public float RotationSpeed = 10.0f;
    private Rigidbody m_rigidBody;


    // Use this for initialization
    void Start ()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_rigidBody.angularVelocity = new Vector3(0f, 0f, RotationSpeed);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.OnGoop(this);
        }
    }
}
