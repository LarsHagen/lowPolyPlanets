using NoiseTest;
using UnityEngine;

namespace Assets.Generators
{
    public class GasGiantGenerator : IGenerator
    {
        private OpenSimplexNoise noise;

        public GeneratorData GetData(Vector3 position)
        {
            float height = (float)noise.Evaluate(position.x * 30, position.y * 30, position.z * 30) * 0.01f;

            float colorNoise = (float)(noise.Evaluate(position.x * 3, position.y * 20, position.z * 3) * 2f + 1) / 2f;
            Color color = Color.Lerp(new Color32(255, 200, 255, 255), new Color32(150, 0, 150, 255), colorNoise);

            return new GeneratorData(height, color);
        }

        public void Init(long seed)
        {
            noise = new OpenSimplexNoise(seed);
        }
    }
}
