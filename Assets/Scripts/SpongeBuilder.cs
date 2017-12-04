using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpongeBuilder : MonoBehaviour {
    public float EdgeLength = 1f;

    MeshFilter _meshFilter;
    Transform top, bottom, left, right;
    private float ColliderDepth = 0.1f;

    private Transform EnsureChild(Vector2 center, string name)
    {
        // add our children
        GameObject go;
        Transform childTransform = transform.Find(name);
        if (childTransform != null)
        {
            go = childTransform.gameObject;
        } else
        {
            go = new GameObject(name);
            go.transform.parent = transform;
        }

        // make sure they have a rigid body
        Rigidbody rb = go.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = go.AddComponent<Rigidbody>();
        }

        // make sure parent is notified of collisions
        ChildCollisionNotifyParent ccnp = go.GetComponent<ChildCollisionNotifyParent>();
        if (ccnp == null)
        {
            ccnp = go.AddComponent<ChildCollisionNotifyParent>();
        }
        
        go.transform.localPosition = center;
        go.layer = 8; // no self collision
        rb.constraints = RigidbodyConstraints.FreezePositionZ |
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationY;

        return go.transform;
    }

    private Vector3[] ComputeVerts()
    {
        return new Vector3[]{
            new Vector2(-EdgeLength/2.0f, top.transform.localPosition.y),
            new Vector2(EdgeLength/2.0f, top.transform.localPosition.y),
            new Vector2(-EdgeLength/2.0f, bottom.transform.localPosition.y),
            new Vector2(EdgeLength/2.0f, bottom.transform.localPosition.y)
        };
    }

    private void EnsureCollider(GameObject target, Vector3 size)
    {
        BoxCollider bc = target.GetComponent<BoxCollider>();
        if (bc == null)
        {
            bc = target.AddComponent<BoxCollider>();
        }

        bc.size = size;
    }

    private void EnsureColliders()
    {
        EnsureCollider(bottom.gameObject, new Vector3(EdgeLength / 2f, ColliderDepth, ColliderDepth));
        EnsureCollider(top.gameObject, new Vector3(EdgeLength / 2f, ColliderDepth, ColliderDepth));
        EnsureCollider(left.gameObject, new Vector3(ColliderDepth, EdgeLength / 2f, ColliderDepth));
        EnsureCollider(right.gameObject, new Vector3(ColliderDepth, EdgeLength / 2f, ColliderDepth));
    }

    private void EnsureConnected(GameObject target, GameObject a)
    {
        SpringJoint[] springs = target.GetComponents<SpringJoint>();
        if (springs == null || springs.Length == 0)
        {
            springs = new SpringJoint[]
            {
                target.AddComponent<SpringJoint>(),
                target.AddComponent<SpringJoint>()
            };
        }

        springs[0].connectedBody = a.GetComponent<Rigidbody>();
        springs[0].spring = 10.0f;

        springs[1].connectedBody = GetComponent<Rigidbody>();
        springs[1].spring = 10.0f;
        springs[1].enableCollision = true;
    }

    private void ConnectWithSprings()
    {
        EnsureConnected(bottom.gameObject, left.gameObject);
        EnsureConnected(left.gameObject, top.gameObject);
        EnsureConnected(top.gameObject, right.gameObject);
        EnsureConnected(right.gameObject, bottom.gameObject);
    }

    private void BuildInitialMesh()
    {
        // set up the verts and tris
        Vector3[] verts = ComputeVerts();

        int iul = 0; int iur = 1;
        int ill = 2; int ilr = 3;

        int[] tris = new int[]
        {
            ill, iul, ilr,
            iul, iur, ilr
        };

        Vector3[] norms = new Vector3[]
        {
            -Vector3.forward, -Vector3.forward,
            -Vector3.forward, -Vector3.forward
        };

        Vector2[] uv = new Vector2[]
        {
            new Vector2(0f, 1f), new Vector2(1f, 1f),
            new Vector2(0f, 0f), new Vector2(1f, 0f)
        };

        Mesh mesh = new Mesh();
        mesh.name = "ComputedMesh";
        mesh.vertices = verts;
        Debug.Log(mesh);
        mesh.normals = norms;
        mesh.uv = uv;
        mesh.triangles = tris;

        _meshFilter.mesh = mesh;
    }

    private void EnsureChildren()
    {
        top = EnsureChild(new Vector2(0f, EdgeLength / 2.0f), "Top");
        bottom = EnsureChild(new Vector2(0f, -EdgeLength / 2.0f), "Bottom");
        left = EnsureChild(new Vector2(-EdgeLength / 2.0f, 0f), "Left");
        right = EnsureChild(new Vector2(EdgeLength / 2.0f, 0f), "Right");
    }

    private void EnsureMembers()
    {
        EnsureChildren();
        _meshFilter = GetComponent<MeshFilter>();
    }

    [ContextMenu("Update Parts")]
    private void UpdateParts()
    {
        EnsureMembers();
        ConnectWithSprings();
        EnsureColliders();
        BuildInitialMesh();
    }

    // Use this for initialization
    void Start () {
        EnsureMembers();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3[] verts = ComputeVerts();
        _meshFilter.mesh.vertices = verts;
    }
}
