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

    [HideInInspector]
    public float caveFactor = .09f;

    [HideInInspector]
    public float caveCutoff = .40f;

    public void Start()
    {
        Generate();
    }

    public void Generate()
    {
        for (int x = 0; x < 16; x++)
        {
            for (int z = 0; z < 16; z++)
            {
                float xComponent = seed + ((transform.position.x + (x * 1f)) * factor);
                float yComponent = seed + ((transform.position.z + (z * 1f)) * factor);
                float noiseFactor = Mathf.PerlinNoise(xComponent, yComponent);
                int stoneLayer = (int)(10f + noiseFactor * 10f);

                for (int y = 0; y < 32; y++)
                {
                    if (y == 0)
                    {
                        blocks[x, y, z] = (byte)Blocks.Bedrock;
                    }
                    else if (y < stoneLayer)
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
                    else
                    {
                        blocks[x, y, z] = (byte)Blocks.Air;
                    }
                }
            }
        }

        genererateCaves();
    }

    public void genererateCaves() 
    {
        for (int x = 0; x < 16; x++)
        {
            for (int z = 0; z < 16; z++)
            {
                for (int y = 0; y < 32; y++)
                {
                    if (y == 0) {
                        continue;
                    }

                    float xComponent = seed + ((transform.position.x + (x * 1f)) * caveFactor);
                    float yComponent = seed + ((transform.position.z + (z * 1f)) * caveFactor);
                    float zComponent = seed + ((transform.position.y + (y * 1f)) * caveFactor);
                    float noiseFactor = Perlin3D(xComponent, yComponent, zComponent);

                    if (noiseFactor < caveCutoff) {
                        blocks[x, y, z] = (byte) Blocks.Air;
                    }
                }
            }
        }


    }

    public static float Perlin3D(float x, float y, float z) 
    {
        float ab = Mathf.PerlinNoise(x, y);
        float bc = Mathf.PerlinNoise(y, z);
        float ac = Mathf.PerlinNoise(x, z);

        float ba = Mathf.PerlinNoise(y, x);
        float cb = Mathf.PerlinNoise(z, y);
        float ca = Mathf.PerlinNoise(z, x);

        float abc = ab + bc + ac + ba + cb + ca;

        return abc / 6f;
    }

    public void DestroyRadius(Vector3 point, float magnitude)
    {
        for (int x = 0; x < 16; x++)
        {
            for (int z = 0; z < 16; z++)
            {
                for (int y = 0; y < 32; y++)
                {
                    Vector3 blockPosition = new Vector3(x * 1f, y * 1f, z * 1f) + transform.position;
                    if (Vector3.Distance(point, blockPosition) < magnitude) {
                        blocks[x, y, z] = (byte)Blocks.Air;
                    }
                }
            }
        }
    }
}
