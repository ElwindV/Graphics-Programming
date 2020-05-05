using UnityEngine;
using System.Collections.Generic;

public class VoxelHandler : MonoBehaviour
{
    public int xChunkCount;
    public int zChunkCount;

    [HideInInspector]
    public GameObject[,] chunks = new GameObject[0, 0];

    [HideInInspector]
    public static VoxelHandler instance = null;

    [HideInInspector]
    public Dictionary<string, Block> blockData;

    public void Start()
    {
        VoxelHandler.instance = this;
        gameObject.GetComponent<Atlas>().GenerateAtlas();
        LoadBlockData();
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
            }
        }
    }

    public void SetBlock(int x, int y, int z, Blocks block = Blocks.Stone, bool updateMeshes = true)
    {
        // DETERMINE CHUNK
        int chunkX = x / 16;
        int chunkZ = z / 16;

        if (chunkX > xChunkCount || chunkX < 0 || chunkZ > zChunkCount || chunkZ < 0)
        {
            return;
        }

        GameObject chunk = chunks[chunkX, chunkZ];

        int localX = x % 16;
        int localZ = z % 16;

        // REMOVE BLOCK
        chunk.GetComponent<Chunk>().blocks[localX, y, localZ] = (byte)block;

        if (!updateMeshes)
        {
            return;
        }

        // UPDATE MESH
        ChunkMesh chunkMesh = chunk.GetComponent<ChunkMesh>();
        chunkMesh.Refresh();

        // UPDATE NEIGHBOURS
        if (localX == 0)
        {
            chunkMesh?.leftChunk?.gameObject?.GetComponent<ChunkMesh>()?.Refresh();
        }
        if (localX == 15)
        {
            chunkMesh?.rightChunk?.gameObject?.GetComponent<ChunkMesh>()?.Refresh();
        }
        if (localZ == 0)
        {
            chunkMesh?.frontChunk?.gameObject?.GetComponent<ChunkMesh>()?.Refresh();
        }
        if (localZ == 15)
        {
            chunkMesh?.backChunk?.gameObject?.GetComponent<ChunkMesh>()?.Refresh();
        }
    }

    private void LoadBlockData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Data/blocks");
        BlockContainer blocksContainer = JsonUtility.FromJson<BlockContainer>(jsonFile.text);

        blockData = new Dictionary<string, Block>();

        foreach (Block block in blocksContainer.blocks)
        {
            blockData[block.name] = block;
        }
    }
}
