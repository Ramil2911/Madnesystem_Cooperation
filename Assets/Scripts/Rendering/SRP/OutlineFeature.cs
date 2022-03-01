/*using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering;

public class OutlineFeature : CustomPass
{
    /*public LayerMask outlineLayer;
    public Color outlineColor = Color.white;
    public float outlineThickness;

    private RTHandle _buffer;
    private Material _material;

    private Shader _shader; //unity's shader loading fix
    
    //cache
    private static readonly int Thickness = Shader.PropertyToID("thickness");
    private static readonly int OutlineBuffer = Shader.PropertyToID("outline_buffer");
    private static readonly int OutlineColor = Shader.PropertyToID("outline_color");

    protected override void Setup(ScriptableRenderContext renderContext, CommandBuffer cmd)
    {
        _shader = Shader.Find("Hidden/Outline");
        _material = CoreUtils.CreateEngineMaterial(_shader);

        // Define the outline buffer
        _buffer = RTHandles.Alloc(
            Vector2.one, TextureXR.slices, dimension: TextureXR.dimension,
            colorFormat: GraphicsFormat.B10G11R11_UFloatPack32,
            // We don't need alpha for this effect
            useDynamicScale: true, name: "Outline Buffer"
        );
    }

    protected override void Execute(CustomPassContext ctx)
    {
        // Render meshes we want to apply the outline effect to in the outline buffer
        CoreUtils.SetRenderTarget(ctx.cmd, _buffer, ClearFlag.Color);
        CustomPassUtils.DrawRenderers(ctx, outlineLayer);

        // Set up outline effect properties
        ctx.propertyBlock.SetColor(OutlineColor, outlineColor);
        ctx.propertyBlock.SetTexture(OutlineBuffer, _buffer);
        ctx.propertyBlock.SetFloat(Thickness, outlineThickness);

        // Render the outline buffer fullscreen
        CoreUtils.SetRenderTarget(ctx.cmd, ctx.cameraColorBuffer, ClearFlag.None);
        CoreUtils.DrawFullScreen(ctx.cmd, _material, ctx.propertyBlock, shaderPassId: 0);
    }

    protected override void Cleanup()
    {
        CoreUtils.Destroy(_material);
        _buffer.Release();
    }#1#
}*/