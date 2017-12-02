using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowOnString : MonoBehaviour {
    public GameObject Target; // what we follow
    public float StringLength = 1.0f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void LateUpdate () {
        Vector2 towardTarget = (Target.transform.position - transform.position);
        float distance = towardTarget.magnitude;
        towardTarget.Normalize();

        if (distance > StringLength)
        {
            float violation = distance - StringLength;

            transform.position += violation * (Vector3)towardTarget;
        }
	}
}
