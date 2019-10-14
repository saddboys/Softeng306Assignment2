using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game
{
    public class Particles
    {
        public static void InitParticleSystem(ParticleSystem particleSystem)
        {
            var renderer = particleSystem.GetComponent<ParticleSystemRenderer>();
            renderer.renderMode = ParticleSystemRenderMode.Billboard;
            renderer.material = GetParticleMaterial();
        }

        private static Material particleMaterial;

        public static Material GetParticleMaterial()
        {
            if (particleMaterial != null) return particleMaterial;

            Shader shader = Shader.Find("Particles/Standard Unlit");
            particleMaterial = new Material(shader);

            // Find the default texture. Who knows where it is located.
            Texture texture = null;
            foreach (Texture potentialTexture in Resources.FindObjectsOfTypeAll<Texture>())
            {
                if (potentialTexture.name == "Default-ParticleSystem")
                {
                    texture = potentialTexture;
                }
            }

            particleMaterial.mainTexture = texture;

            // Set fade blend mode. Unfortunately, we need to manually configure its values
            // according to the source code.
            particleMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            particleMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            particleMaterial.SetInt("_ZWrite", 0);
            particleMaterial.DisableKeyword("_ALPHATEST_ON");
            particleMaterial.EnableKeyword("_ALPHABLEND_ON");
            particleMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            particleMaterial.renderQueue = 3000;

            return particleMaterial;
        }
    }
}
