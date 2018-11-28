using NoiseTest;
using UnityEngine;

namespace Assets.Generators
{
    public class EarthLikeGenerator : IGenerator
    {
        private OpenSimplexNoise noise;

        public GeneratorData GetData(Vector3 position)
        {
            float noiseValue = (float)(noise.Evaluate(position.x * 3, position.y * 3, position.z * 3) - 0.2f) * 0.2f;
            float details = (float)noise.Evaluate(position.x * 30, position.y * 30, position.z * 30) * 0.05f;
            
            float height = noiseValue + details;
            
            if (height < 0f)
            {
                height *= 0.3f;
            }

            Color color = new Color32(75, 165, 0, 255);
            if (height < 0)
            {
                color = new Color32(255, 231, 0, 255);
            }
            else if (height > 0.06f)
            {
                color = new Color32(50, 50, 50, 255);
            }

            if (position.y > 0.4f || position.y < -0.4f)
            {
                color = Color.white;
            }

            return new GeneratorData(height, color);
        }

        public void Init(long seed)
        {
            noise = new OpenSimplexNoise(seed);
        }
    }
}
