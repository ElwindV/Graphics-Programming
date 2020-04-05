using UnityEngine;

public class Chunk : MonoBehaviour
{
    [HideInInspector]
    public byte[,,] blocks = new byte[16, 32, 16];

    public int ChunkWidth
    {
        get {
            return blocks.GetLength(0);
        }
    }

    public int ChunkHeight {
        get {
            return blocks.GetLength(1);
        }
    }

    public int ChunkDepth
    {
        get
        {
            return blocks.GetLength(2);
        }
    }

    [HideInInspector]
    public int seed = 4113;

    [HideInInspector]
    public float factor = .07f;

    [HideInInspector]
    public int x;

    [HideInInspector]
    public int z;

    void Start()
    {
        for (int x = 0; x < 16; x++) {
            for (int z = 0; z < 16; z++) {
                float xComponent = seed + ((transform.position.x + (x * 1f)) * factor);
                float yComponent = seed + ((transform.position.z + (z * 1f)) * factor);
                float noiseFactor = Mathf.PerlinNoise(xComponent, yComponent);
                int stoneLayer = (int)(10f + noiseFactor * 10f);

                for (int y = 0; y < 32; y++) {
                    if (y < stoneLayer)
                    {
                        blocks[x, y, z] = (byte)Blocks.Stone;
                    }
                    else if (y < stoneLayer + 3)
                    {
                        blocks[x, y, z] = (byte)Blocks.Dirt;
                    }
                    else if (y < stoneLayer + 4)
                    {
                        blocks[x, y, z] = (byte)Blocks.Grass;
                    }
                    else {
                        blocks[x, y, z] = (byte)Blocks.Air;
                    }

                    //Vector2 blocksCoordinate = new Vector2(transform.position.x + x * 1f, transform.position.y + y * 1f);
                    //Vector2 holeCoordinate = new Vector2(10f, 5f);
                    //if (Vector2.Distance(blocksCoordinate, holeCoordinate) < 3f)
                    //{
                    //    blocks[x, y, z] = (byte)Blocks.Air;
                    //}
                }
            }
        }
    }


}
