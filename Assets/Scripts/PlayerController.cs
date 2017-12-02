using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float JumpHeight = 10.0f;

    private Rigidbody2D m_rigidBody;
    private GameObject m_world;
    private bool m_grounded = true;
    private float m_jumpImpulse = 0.0f;

	// Use this for initialization
	void Start () {     
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_world = GameObject.Find("World");
        m_jumpImpulse = m_rigidBody.mass * Mathf.Sqrt(2.0f * Mathf.Abs(Physics.gravity.y) * JumpHeight / 3.0f);
	}
	
    Vector3 MouseInWorld()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return ray.origin;
    }

    Vector2 ToMouse()
    {
        Vector2 toMouse = MouseInWorld() - transform.position;
        toMouse.Normalize();
        return toMouse;
    }

    
    void FixedUpdate () {
        //bool jumpStart = Input.GetButtonDown("Jump");
        bool jumpPressed = Input.GetButton("Jump");

        if (m_grounded && jumpPressed)
        {
            Vector2 jumpForce = m_jumpImpulse * ToMouse();
            Debug.Log(jumpForce.ToString());
            m_rigidBody.AddForce(jumpForce, ForceMode2D.Impulse);
            m_grounded = false;
            m_rigidBody.gravityScale = 1.0f;
        }
	}

    // Update is called once per frame
    private void Update()
    {
        Vector2 rayEnd = (Vector2)transform.position + 3.0f * ToMouse();
        Debug.DrawLine(transform.position, rayEnd, Color.red);
    }

    void UpdateGrounded(Collider2D collider)
    {
        GameObject go = collider.gameObject;
        while (go != null)
        {
            if (go == m_world)
            {
                Debug.Log("triggered");
                m_grounded = true;
                m_rigidBody.velocity = Vector2.zero;
                m_rigidBody.gravityScale = 0.0f;
                return;
            }

            go = go.transform.parent.gameObject;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        UpdateGrounded(collision.collider);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("on collision");
        UpdateGrounded(collision.collider);
    }
}
