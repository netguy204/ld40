using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpongeMesh : MonoBehaviour {
    MeshFilter _meshFilter;
    Mesh _mesh;

    Transform ur, lr, ul, ll;

	// Use this for initialization
	void Start () {
        _meshFilter = GetComponent<MeshFilter>();
        ur = transform.Find("UR");
        lr = transform.Find("LR");
        ul = transform.Find("UL");
        ll = transform.Find("LL");

        // set up the verts and tris
        Vector3[] verts = new Vector3[]
        {
            ul.localPosition, ur.localPosition,
            ll.localPosition, lr.localPosition
        };

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

        _mesh = new Mesh();
        _mesh.vertices = verts;
        _mesh.normals = norms;
        _mesh.uv = uv;
        _mesh.triangles = tris;
  
        _meshFilter.mesh = _mesh;
        Debug.Log(_mesh.ToString());
	}
	
	// Update is called once per frame
	void Update () {
        // update the verts. everything else is static
        Vector3[] verts = new Vector3[]
        {
            ul.localPosition, ur.localPosition,
            ll.localPosition, lr.localPosition
        };

        _meshFilter.mesh.vertices = verts;
    }
}
