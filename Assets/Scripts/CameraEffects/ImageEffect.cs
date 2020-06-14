using UnityEngine;

[ExecuteInEditMode]
public class ImageEffect : MonoBehaviour
{
    [SerializeField]
    private Material _effect;

    public void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        Graphics.Blit(src, dst, _effect);
    }
}
