using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
    public float JumpHeight = 10.0f;

    private Rigidbody m_rigidBody;
    private GameObject m_world;
    private bool m_grounded = true;
    private float m_jumpImpulse = 0.0f;
    private LineRenderer m_lineRenderer;

	// Use this for initialization
	void Start () {
        m_rigidBody = GetComponent<Rigidbody>();
        m_world = GameObject.Find("World");
        m_jumpImpulse = m_rigidBody.mass * Mathf.Sqrt(2.0f * Mathf.Abs(Physics.gravity.y) * JumpHeight / 3.0f);
        m_lineRenderer = GetComponent<LineRenderer>();
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
            m_rigidBody.AddForce(jumpForce, ForceMode.Impulse);
            m_grounded = false;
            m_rigidBody.useGravity = true;
        }
	}

    // Update is called once per frame
    private void Update()
    {
        Vector2 rayEnd = (Vector2)transform.position + 3.0f * ToMouse();
        Debug.DrawLine(transform.position, rayEnd, Color.red);
        m_lineRenderer.SetPosition(0, transform.position);
        m_lineRenderer.SetPosition(1, MouseInWorld());
    }

    void UpdateGrounded(Collider collider)
    {
        GameObject go = collider.gameObject;
        while (go != null)
        {
            if (go == m_world)
            {
                m_grounded = true;
                m_rigidBody.velocity = Vector2.zero;
                m_rigidBody.useGravity = false;
                return;
            }
            
            go = go.transform.parent.gameObject;
        }
    }

    public void NotifyChildCollision(Collision collision)
    {
        UpdateGrounded(collision.collider);
    }

    private void OnCollisionStay(Collision collision)
    {
        UpdateGrounded(collision.collider);
    }

    private void OnCollisionEnter(Collision collision)
    {
        UpdateGrounded(collision.collider);
    }

    public void OnGoop(Goop goop)
    {
        Destroy(goop.gameObject);
        transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
    }

    public void OnWinTriggered(WinTrigger win)
    {
        Debug.Log("win triggered");
        Destroy(win.gameObject);
        SceneManager.LoadScene(2);
    }
}
