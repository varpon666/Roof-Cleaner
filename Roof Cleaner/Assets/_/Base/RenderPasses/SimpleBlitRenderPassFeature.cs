using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SimpleBlitRenderPassFeature : ScriptableRendererFeature
{
    class SimpleBlitRenderPass : ScriptableRenderPass
    {

        public RenderTargetIdentifier source;

        private Material material;
        private RenderTargetHandle tempRenderTargetHandler;

        public SimpleBlitRenderPass(Material material) {
            this.material = material;

            tempRenderTargetHandler.Init("_TemporaryColorTexture");
        }

        // This method is called before executing the render pass.
        // It can be used to configure render targets and their clear state. Also to create temporary render target textures.
        // When empty this render pass will render to the active camera render target.
        // You should never call CommandBuffer.SetRenderTarget. Instead call <c>ConfigureTarget</c> and <c>ConfigureClear</c>.
        // The render pipeline will ensure target setup and clearing happens in an performance manner.
        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
        }

        // Here you can implement the rendering logic.
        // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
        // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
        // You don't have to call ScriptableRenderContext.submit, the render pipeline will call it at specific points in the pipeline.
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer commandBuffer = CommandBufferPool.Get("SimpleBlitRenderPass");

            commandBuffer.GetTemporaryRT(tempRenderTargetHandler.id, renderingData.cameraData.cameraTargetDescriptor);
            Blit(commandBuffer, source, tempRenderTargetHandler.Identifier(), material);
            Blit(commandBuffer, tempRenderTargetHandler.Identifier(), source);

            context.ExecuteCommandBuffer(commandBuffer);
            CommandBufferPool.Release(commandBuffer);
        }

        /// Cleanup any allocated resources that were created during the execution of this render pass.
        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(tempRenderTargetHandler.id);
        }
    }


    [System.Serializable]
    public class SimpleBlitSettings {

        public Material material = null;
        public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        public ClearFlag clearFlag = ClearFlag.None;
        public Color clearColor = Color.black;

    }


    public SimpleBlitSettings settings = new SimpleBlitSettings();

    SimpleBlitRenderPass simpleBlitRenderPass;

    public override void Create()
    {
        simpleBlitRenderPass = new SimpleBlitRenderPass(settings.material);

        // Configures where the render pass should be injected.
        simpleBlitRenderPass.renderPassEvent = settings.renderPassEvent;
        simpleBlitRenderPass.ConfigureClear(settings.clearFlag, settings.clearColor);
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        simpleBlitRenderPass.source = renderer.cameraColorTarget;
        renderer.EnqueuePass(simpleBlitRenderPass);
    }
}


