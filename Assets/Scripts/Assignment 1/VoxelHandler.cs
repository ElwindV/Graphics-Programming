using UnityEngine;

public class VoxelHandler : MonoBehaviour
{
    public int xChunkCount = 1;
    public int zChunkCount = 1;

    protected GameObject[,] chunks = new GameObject[5,5];

    public void Start()
    {
        chunks = new GameObject[xChunkCount, zChunkCount];

        for (int x = 0; x < chunks.GetLength(0); x++) {
            for (int z = 0; z < chunks.GetLength(1); z++) {
                GameObject chunk = new GameObject();

                chunk.transform.SetParent(this.transform);
                chunk.name = $"Chunk {x}:{z}";
                chunk.transform.position = new Vector3(16 * x, 0, 16 * z);
                chunk.AddComponent<Chunk>();
                chunk.AddComponent<ChunkMesh>();

                chunks[x, z] = chunk;
            }
        }

    }


}
