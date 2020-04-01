using UnityEngine;

public class BasicMesh : MonoBehaviour
{
    protected MeshFilter meshFilter;

    protected Vector3[] vertices = new Vector3[4]
    {
        new Vector3(0, 0, 0),
        new Vector3(1, 0, 0),
        new Vector3(0, 1, 0),
        new Vector3(1, 1, 0)
    };

    int[] triangles = new int[6]
    {
        // lower left triangle
        0, 2, 1,
        // upper right triangle
        2, 3, 1
    };

    Vector3[] normals = new Vector3[4]
    {
        -Vector3.forward,
        -Vector3.forward,
        -Vector3.forward,
        -Vector3.forward
    };


    Vector2[] uv = new Vector2[4]
    {
        new Vector2(0, 0),
        new Vector2(1, 0),
        new Vector2(0, 1),
        new Vector2(1, 1)
    };

    public void Start()
    {
        meshFilter = GetComponent<MeshFilter>();

        Mesh mesh = new Mesh();

        meshFilter.mesh = mesh;

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = uv;
    }

}
