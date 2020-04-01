using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    This was me mostly following https://docs.unity3d.com/ScriptReference/Mesh.html trying to create a barrel
*/
public class ProceduralMesh : MonoBehaviour
{
    [Range(3, 32)] public int sides = 8;
    [Range(2, 32)] public int layers = 2;
    [Range(.1f, 2f)] public float radius = 1f;
    [Range(.1f, 2f)] public float height = 1f;

    [Range(-0.1f, 0.3f)] public float bloatness = 0f;

    protected MeshFilter meshFilter;

    protected Vector3[] vertices;
    int[] triangles;
    Vector3[] normals;
    Vector2[] uv;

    public void Start()
    {
        GenerateNewMesh();
    }

    public void GenerateNewMesh()
    {
        meshFilter = GetComponent<MeshFilter>();

        vertices = new Vector3[sides * layers];
        for (int i = 0; i < layers; i++)
        {
            for (int j = 0; j < sides; j++)
            {
                float progress = ((i) * 1f) / (1f * layers);
                float bloat = bloatness * Mathf.Sin(progress * (Mathf.PI));
                float tmpRadius = radius + radius * bloat;

                vertices[i * sides + j] = new Vector3(
                    tmpRadius * Mathf.Cos((j * -1f) / (sides * 1f) * Mathf.PI * 2f),
                   (i * 1f) * (height * 1f) / ((layers - 1) * 1f),
                    tmpRadius * Mathf.Sin((j * -1f) / (sides * 1f) * Mathf.PI * 2f)
                );
            }
        }

        triangles = new int[sides * layers * 2 * 3];
        int totalCounter = 0;
        for (int i = 0; i < layers - 1; i++)
        {
            for (int j = 0; j < sides; j++)
            {
                triangles[totalCounter++] = j + (i * sides);
                triangles[totalCounter++] = (j + 1) % sides + (i * sides);
                triangles[totalCounter++] = (j + 1) % sides + ((i + 1) * sides);
                triangles[totalCounter++] = j + (i * sides);
                triangles[totalCounter++] = (j + 1) % sides + ((i + 1) * sides);
                triangles[totalCounter++] = (j) + ((i + 1) * sides);
            }
        }

        normals = new Vector3[vertices.Length];
        for (int i = 0; i < sides * layers; i++)
        {
            normals[i] = height / 2f * vertices[i];
        }

        uv = new Vector2[vertices.Length];
        //for (int x = 0; x < sides; x++) {
        //    for (int y = 0; y < layers; y++) {
        //        uv[(x * sides) + y] = new Vector2(x,);
        //    }
        //}
        for (int i = 0; i < sides * layers; i++)
        {
            float x = ((i % sides) * 1f) / (sides * 1f);
            float y = ((i / sides) * 1f) / (sides * 1f);
            uv[i] = new Vector2(x, y);
        }

        Mesh mesh = new Mesh();

        meshFilter.mesh = mesh;

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = uv;
    }

}
