﻿using System.Collections.Generic;
using UnityEngine;

public class ChunkMesh : MonoBehaviour
{
    protected MeshFilter meshFilter;
    protected Mesh mesh;
    protected MeshRenderer meshRenderer;

    protected Chunk chunk;
    protected Material material;

    protected List<Vector3> vertexList;
    protected List<int> triangleList;
    protected List<Vector3> normalList;
    protected List<Vector2> uvList;

    void Start()
    {
        meshFilter = this.gameObject.AddComponent<MeshFilter>();
        meshRenderer = this.gameObject.AddComponent<MeshRenderer>();

        chunk = this.gameObject.GetComponent<Chunk>();

        material = Resources.Load<Material>("Materials/Voxel");
        meshRenderer.material = this.material;

        Refresh();
    }

    public void Refresh()
    {
        Mesh mesh = new Mesh();

        vertexList = new List<Vector3>();
        triangleList = new List<int>();
        normalList = new List<Vector3>();
        uvList = new List<Vector2>();

        for (int x = 0; x < chunk.blocks.GetLength(0); x++) {
            for (int y = 0; y < chunk.blocks.GetLength(1); y++) {
                for (int z = 0; z < chunk.blocks.GetLength(2); z++) {
                    byte block = chunk.blocks[x, y, z];
                    Vector3 currentPosition = new Vector3(x, y, z);

                    if (block == (byte)Blocks.Air)
                    {
                        continue;
                    }

                    byte rightBlock = ((x + 1 != chunk.blocks.GetLength(0)) ? chunk.blocks[x + 1, y, z] : (byte)Blocks.Air);
                    byte leftBlock = ((x - 1 >= 0) ? chunk.blocks[x - 1, y, z] : (byte)Blocks.Air);

                    byte topBlock       = ((y + 1 != chunk.blocks.GetLength(1)) ? chunk.blocks[x, y + 1, z] : (byte)Blocks.Air);
                    byte bottomBlock    = ((y - 1 >= 0) ? chunk.blocks[x, y - 1, z] : (byte) Blocks.Air);

                    byte backBlock      = ((z + 1 != chunk.blocks.GetLength(2)) ? chunk.blocks[x, y, z + 1] : (byte)Blocks.Air);
                    byte frontBlock     = ((z - 1 >= 0) ? chunk.blocks[x, y, z - 1] : (byte)Blocks.Air);

                    HandleRight(ref block, ref rightBlock, ref currentPosition);
                    HandleLeft(ref block, ref leftBlock, ref currentPosition);
                    HandleTop(ref block, ref topBlock, ref currentPosition);
                    HandleBottom(ref block, ref bottomBlock, ref currentPosition);
                    HandleBack(ref block, ref backBlock, ref currentPosition);
                    HandleFront(ref block, ref frontBlock, ref currentPosition);
                }
            }
        }

        meshFilter.mesh = mesh;
        mesh.vertices = vertexList.ToArray();
        mesh.triangles = triangleList.ToArray();
        mesh.normals = normalList.ToArray();
        mesh.uv = uvList.ToArray();
    }

    protected void AddFaceNormals(Vector3 direction) {
        for (short i = 0; i < 4; i++) {
            normalList.Add(direction);
        }
    }

    protected void AddTriangles(int baseVertexNumber) 
    {
        triangleList.Add(baseVertexNumber + 0);
        triangleList.Add(baseVertexNumber + 3);
        triangleList.Add(baseVertexNumber + 1);

        triangleList.Add(baseVertexNumber + 0);
        triangleList.Add(baseVertexNumber + 2);
        triangleList.Add(baseVertexNumber + 3);
    }

    protected void AddUVs(byte block, Sides side) 
    {
        uvList.Add(0.5f * Vector2.zero);
        uvList.Add(0.5f * Vector2.right);
        uvList.Add(0.5f * Vector2.up);
        uvList.Add(0.5f * Vector2.right + 0.5f * Vector2.up);
    }

    protected void HandleRight(ref byte block, ref byte rightBlock, ref Vector3 currentPosition) 
    {
        if (rightBlock != (byte)Blocks.Air) {
            return;
        }

        int size = vertexList.Count;
        vertexList.Add(currentPosition + Vector3.right);                                    // + 0
        vertexList.Add(currentPosition + Vector3.right + Vector3.forward);                  // + 1
        vertexList.Add(currentPosition + Vector3.right + Vector3.up);                       // + 2
        vertexList.Add(currentPosition + Vector3.right + Vector3.forward + Vector3.up);     // + 3

        AddFaceNormals(Vector3.right);
        AddTriangles(size);
        AddUVs(block, Sides.Right);
    }

    protected void HandleLeft(ref byte block, ref byte leftBlock, ref Vector3 currentPosition) 
    {
        if (leftBlock != (byte)Blocks.Air)
        {
            return;
        }

        int size = vertexList.Count;
        vertexList.Add(currentPosition + Vector3.forward);                  // + 0
        vertexList.Add(currentPosition);                                    // + 1
        vertexList.Add(currentPosition + Vector3.forward + Vector3.up);     // + 2
        vertexList.Add(currentPosition + Vector3.up);                       // + 3

        AddFaceNormals(Vector3.left);
        AddTriangles(size);
        AddUVs(block, Sides.Left);
    }

    protected void HandleTop(ref byte block, ref byte topBlock, ref Vector3 currentPosition) 
    {
        if (topBlock != (byte)Blocks.Air)
        {
            return;
        }

        int size = vertexList.Count;
        vertexList.Add(currentPosition + Vector3.up);                                    // + 0
        vertexList.Add(currentPosition + Vector3.up + Vector3.right);                    // + 1
        vertexList.Add(currentPosition + Vector3.up + Vector3.forward);                  // + 2
        vertexList.Add(currentPosition + Vector3.up + Vector3.right + Vector3.forward);  // + 3

        AddFaceNormals(Vector3.up);
        AddTriangles(size);
        AddUVs(block, Sides.Top);
    }

    protected void HandleBottom(ref byte block, ref byte bottomBlock, ref Vector3 currentPosition) 
    {
        if (bottomBlock != (byte)Blocks.Air)
        {
            return;
        }

        int size = vertexList.Count;
        vertexList.Add(currentPosition + Vector3.right);                    // + 0
        vertexList.Add(currentPosition);                                    // + 1
        vertexList.Add(currentPosition + Vector3.right + Vector3.forward);  // + 2
        vertexList.Add(currentPosition + Vector3.forward);                  // + 3

        AddFaceNormals(Vector3.down);
        AddTriangles(size);
        AddUVs(block, Sides.Bottom);

    }

    protected void HandleBack(ref byte block, ref byte backBlock, ref Vector3 currentPosition) 
    {
        if (backBlock != (byte)Blocks.Air)
        {
            return;
        }

        int size = vertexList.Count;
        vertexList.Add(currentPosition + Vector3.forward + Vector3.right);                   // + 0
        vertexList.Add(currentPosition + Vector3.forward);                                   // + 1
        vertexList.Add(currentPosition + Vector3.forward + Vector3.right + Vector3.up);      // + 2
        vertexList.Add(currentPosition + Vector3.forward + Vector3.up);                      // + 3

        AddFaceNormals(Vector3.forward);
        AddTriangles(size);
        AddUVs(block, Sides.Back);
    }

    protected void HandleFront(ref byte block, ref byte frontBlock, ref Vector3 currentPosition) 
    {
        if (frontBlock != (byte)Blocks.Air)
        {
            return;
        }

        int size = vertexList.Count;
        vertexList.Add(currentPosition);                                 // + 0
        vertexList.Add(currentPosition + Vector3.right);                 // + 1
        vertexList.Add(currentPosition + Vector3.up);                    // + 2
        vertexList.Add(currentPosition + Vector3.right + Vector3.up);    // + 3

        AddFaceNormals(Vector3.back);
        AddTriangles(size);
        AddUVs(block, Sides.Front);
    }
}