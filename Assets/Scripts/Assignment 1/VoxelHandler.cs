using UnityEngine;

public class VoxelHandler : MonoBehaviour
{
    public int xChunkCount;
    public int zChunkCount;

    protected GameObject[,] chunks = new GameObject[0,0];

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
