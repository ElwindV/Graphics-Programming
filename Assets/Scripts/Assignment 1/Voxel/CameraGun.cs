using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGun : MonoBehaviour
{
    public new Camera camera;

    public VoxelHandler voxelHandler;

    public float explosionSize = 5f;


    public void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Mine();
        }
        if (Input.GetMouseButtonDown(1))
        {
            Refresh();
        }
    }

    protected void Mine()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out hit, 100f))
        {
            return;
        }

        int xChunkCount = voxelHandler.chunks.GetLength(0);
        int zChunkCount = voxelHandler.chunks.GetLength(1);
        int chunkOverlap = (int)(16f / explosionSize) + 2;

        int xExplosionChunk = (int)hit.transform.position.x / 16;
        int zExplosionChunk = (int)hit.transform.position.z / 16;

        for (int x = Mathf.Max(xExplosionChunk - chunkOverlap, 0) ; x <= xExplosionChunk + chunkOverlap && x < xChunkCount; x++)
        {
            for (int z = Mathf.Max(zExplosionChunk - chunkOverlap, 0); z <= zExplosionChunk + chunkOverlap && z < zChunkCount; z++)
            {
                GameObject chunk = voxelHandler.chunks[x, z];
                chunk.GetComponent<Chunk>().DestroyRadius(hit.point, explosionSize);
                chunk.GetComponent<ChunkMesh>().Refresh();
            }
        }
    }

    protected void Refresh() 
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out hit, 100f))
        {
            return;
        }

        GameObject chunk = hit.transform.gameObject;
        chunk.GetComponent<ChunkMesh>().Refresh();
    }
}
