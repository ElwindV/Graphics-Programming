using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atlas : MonoBehaviour
{
    public Material material;

    public static Dictionary<string, Vector2> uvs;
    public static int dimensions = 0;

    private Texture2D[] textures;

    private readonly int textureWidth = 16;
    private readonly int textureHeight = 16;

    public void GenerateAtlas()
    {
        uvs = new Dictionary<string, Vector2>();
        textures = Resources.LoadAll<Texture2D>("Atlas");

        int textureCount = textures.Length;

        dimensions = GetAtlasDimension(textureCount);

        Texture2D atlas = new Texture2D(textureWidth * dimensions, textureHeight * dimensions)
        {
            anisoLevel = 1,
            filterMode = FilterMode.Point
        };

        for (int i = 0; i < textures.Length; i++) {
            Texture2D texture = textures[i];

            int horizontalAtlasOffset = (i % dimensions) * textureWidth;
            int verticalAtlasOffset = (i / dimensions) * textureHeight;

            int textureX = i % dimensions;
            int textureY = i / dimensions;

            uvs.Add(
                texture.name,
                new Vector2((textureX * 1f) / (dimensions * 1f), (textureY * 1f) / (dimensions * 1f))
            );

            Color[] pixels = texture.GetPixels(0, 0, texture.width, texture.height);
            for (int y = 0; y < texture.height; y++)
            {
                for (int x = 0; x < texture.width; x++)
                {
                    atlas.SetPixel(x + horizontalAtlasOffset, y + verticalAtlasOffset, pixels[x + y * 16]);
                }
            }
        }
        atlas.Apply();

        material.mainTexture = atlas;
    }

    public int GetAtlasDimension(int count) => (int)Mathf.Pow(2, Mathf.Ceil(Mathf.Log(count) / Mathf.Log(4)));
}
