    using UnityEngine;
    public class MandelbrotSystem : MonoBehaviour
    {
        public ComputeShader MandelbrotShader;
        public RenderTexture result;
        private int kernel;
        [Range(-3,3)]
        public float X, Y;
        public int  Z, Multiple;
        //   public Material material;
        private void Start()
        {
            kernel = MandelbrotShader.FindKernel("CSMain");
            result = new RenderTexture(1024, 1024, 24);
            result.enableRandomWrite = true;
            result.Create();
            MandelbrotShader.SetTexture(kernel, "Result", result);
            MandelbrotShader.Dispatch(kernel, 1024 / 8, 1024 / 8, 1);
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            Render(destination);
        }
        private void Render(RenderTexture destination)
        {
            // Make sure we have a current render target
            InitRenderTexture();
            // Set the target and dispatch the compute shader
            int screenY = Screen.height;
            int screenX = Screen.width;
            MandelbrotShader.SetTexture(kernel, "Result", result);
            MandelbrotShader.SetFloat("X", X);
            MandelbrotShader.SetFloat("Y", Y);
            MandelbrotShader.SetInt("Z", Z);
            MandelbrotShader.SetInt("Multiple", Multiple);
            MandelbrotShader.SetInt("textureX", screenX);
            MandelbrotShader.SetInt("textureY", screenY);
            int threadGroupsX = Mathf.CeilToInt(screenX / 8.0f);
            int threadGroupsY = Mathf.CeilToInt(screenY / 8.0f);
            MandelbrotShader.Dispatch(kernel, threadGroupsX, threadGroupsY, 1);
            // Blit the result texture to the screen
            Graphics.Blit(result, destination);
        }
        private void InitRenderTexture()
        {
            if (result == null || result.width != Screen.width || result.height != Screen.height)
            {
                // Release render texture if we already have one
                if (result != null)
                    result.Release();
                // Get a render target for Ray Tracing
                result = new RenderTexture(Screen.width, Screen.height, 0,
                    RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
                result.enableRandomWrite = true;
                result.Create();
            }
        }
    }