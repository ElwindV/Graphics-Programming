using UnityEngine;

public class Chunk : MonoBehaviour
{
    [HideInInspector]
    public byte[,,] blocks = new byte[16,32,16];

    void Start()
    {
        for (int x = 0; x < 16; x++) {
            for (int y = 0; y < 32; y++) {
                for (int z = 0; z < 16; z++) {
                    if (y < 16)
                    {
                        blocks[x, y, z] = (byte)Blocks.Stone;
                    }
                    else if (y < 20)
                    {
                        blocks[x, y, z] = (byte)Blocks.Dirt;
                    }
                    else if (y < 21)
                    {
                        blocks[x, y, z] = (byte)Blocks.Grass;
                    }
                    else {
                        blocks[x, y, z] = (byte)Blocks.Air;
                    }
                }
            }
        }
    }


}
