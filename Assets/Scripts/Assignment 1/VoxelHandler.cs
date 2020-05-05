using UnityEngine;
using System.Collections.Generic;

public class VoxelHandler : MonoBehaviour
{
    public int xChunkCount;
    public int zChunkCount;

    public int treesPerChunk = 4;

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
                chunkComponent.Generate();

                chunks[x, z] = chunk;
            }
        }

        float turnfraction = (1f + Mathf.Sqrt(5)) / 2f;

        int numberOfTrees = xChunkCount * zChunkCount * treesPerChunk;
        for (int i = 0; i < numberOfTrees; i++) {
            float distance = i / (numberOfTrees - 1f);
            float angle = 2 * Mathf.PI * turnfraction * i;

            float x = 64f + (64f * (distance * Mathf.Cos(angle)));
            float z = 64f + (64f * (distance * Mathf.Sin(angle)));

            PlaceTree((int) x, (int) z, false);
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

    public void PlaceTree(int x, int z, bool updateMeshes = false)
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

        Chunk chunkObject = chunk.GetComponent<Chunk>();
        // DETERMINE BEGIN

        int? root = null;
        for (int y = chunkObject.ChunkHeight - 1; y >= 0; y--) {
            byte block = chunkObject.blocks[localX, y, localZ];

            if (block == (byte) Blocks.Grass) {
                root = y + 1;
            }
        }

        if (root == null) {
            return;
        }

        for (int y = (int) root, i = 0; y < root + 8; y++, i++) {
            SetBlock(x, y, z, Blocks.Log, updateMeshes);
            if (i >= 3) {
                SetBlock(x-1, y, z, Blocks.Leaf, updateMeshes);
                SetBlock(x+1, y, z, Blocks.Leaf, updateMeshes);
                SetBlock(x, y, z-1, Blocks.Leaf, updateMeshes);
                SetBlock(x, y, z+1, Blocks.Leaf, updateMeshes);
            }
            if (i == 5) {
                SetBlock(x - 1, y, z - 1, Blocks.Leaf, updateMeshes);
                SetBlock(x + 1, y, z - 1, Blocks.Leaf, updateMeshes);
                SetBlock(x - 1, y, z + 1, Blocks.Leaf, updateMeshes);
                SetBlock(x + 1, y, z + 1, Blocks.Leaf, updateMeshes);
            }
            SetBlock(x, y + 1, z, Blocks.Leaf, updateMeshes);
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
