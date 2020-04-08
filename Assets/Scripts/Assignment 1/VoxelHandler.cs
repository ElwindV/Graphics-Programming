using UnityEngine;

public class VoxelHandler : MonoBehaviour
{
    public int xChunkCount;
    public int zChunkCount;

    [HideInInspector]
    public GameObject[,] chunks = new GameObject[0,0];

    public void Start()
    {
        gameObject.GetComponent<Atlas>().GenerateAtlas();

        chunks = new GameObject[xChunkCount, zChunkCount];

        for (int x = 0; x < chunks.GetLength(0); x++)
        {
            for (int z = 0; z < chunks.GetLength(1); z++)
            {
                GameObject chunk = new GameObject();

                chunk.transform.SetParent(this.transform);
                chunk.name = $"Chunk {x}:{z}";
                chunk.transform.position = new Vector3(16 * x, 0, 16 * z);

                Chunk chunkComponent = chunk.AddComponent<Chunk>();
                chunkComponent.x = x;
                chunkComponent.z = z;

                chunks[x, z] = chunk;
            }
        }

        // This loop is seperate since all Meshes should be generated AFTER the chunks
        for (int x = 0; x < chunks.GetLength(0); x++)
        {
            for (int z = 0; z < chunks.GetLength(1); z++)
            {
                ChunkMesh chunkMesh = chunks[x, z].AddComponent<ChunkMesh>();
                chunkMesh.voxelHandler = this;
            }
        }

    }
}
