using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildCollisionNotifyParent : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void NotifyParent(Collision collision)
    {
        PlayerController player = transform.parent.gameObject.GetComponent<PlayerController>();
        player.NotifyChildCollision(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        NotifyParent(collision);
    }

    private void OnCollisionEnter(Collision collision)
    {
        NotifyParent(collision);
    }

    public void OnGoop(Goop goop)
    {
        PlayerController player = transform.parent.gameObject.GetComponent<PlayerController>();
        player.OnGoop(goop);
    }
}
