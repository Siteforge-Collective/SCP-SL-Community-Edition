using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public static class PostProcessUtils
{
    private static readonly int s_MainTex = Shader.PropertyToID("_MainTex");

    public static void BlitFullScreen(CommandBuffer cmd, RTHandle source, RTHandle destination, Material material, int pass = 0)
    {
        if (material != null)
        {
            material.SetTexture(s_MainTex, source);
            HDUtils.DrawFullScreen(cmd, material, destination, null, pass);
        }
    }
}