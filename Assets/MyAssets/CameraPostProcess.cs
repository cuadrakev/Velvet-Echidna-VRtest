using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPostProcess : MonoBehaviour
{
    private RenderTexture texture;
    private RenderTexture normalTexture;
    private Material postProcessMaterial;

    void Start()
    {
        Camera camera = gameObject.GetComponent<Camera>();
        texture = new RenderTexture(camera.pixelWidth, camera.pixelHeight, 24, RenderTextureFormat.Default);
        normalTexture = new RenderTexture(camera.pixelWidth, camera.pixelHeight, 0, RenderTextureFormat.Default);

        postProcessMaterial = new Material(Shader.Find("MyShaders/Outline"));
    }

    void OnPreRender()
    {
        RenderTexture.active = normalTexture;
        GL.Clear(false, true, new Color(0, 0, 1));
        RenderBuffer[] colorBuffers = new RenderBuffer[] { texture.colorBuffer, normalTexture.colorBuffer };
        gameObject.GetComponent<Camera>().SetTargetBuffers(colorBuffers, texture.depthBuffer);
    }

    void OnPostRender()
    {
        postProcessMaterial.SetTexture("_NormalTex", normalTexture);
        Graphics.Blit(texture, null as RenderTexture, postProcessMaterial);
    }
}
