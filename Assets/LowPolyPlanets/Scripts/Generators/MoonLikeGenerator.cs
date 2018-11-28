using NoiseTest;
using System;
using UnityEngine;

namespace Assets.Generators
{
    public class MoonLikeGenerator : IGenerator
    {
        private OpenSimplexNoise noise;

        public GeneratorData GetData(Vector3 position)
        {
            float crators1 = (float)noise.Evaluate(position.x * 2, position.y * 2, position.z * 2);

            if (crators1 > -0.4f)
                crators1 = 0;
            else
                crators1 += 0.4f;

            crators1 *= 0.7f;

            float crators2 = (float)noise.Evaluate(position.x * 4, position.y * 4, position.z * 4);
            if (crators2 > -0.4f)
                crators2 = 0;
            else
                crators2 += 0.4f;

            float crators = Mathf.Min(crators1, crators2);

            float details = (float)noise.Evaluate(position.x * 30, position.y * 30, position.z * 30) * 0.025f;
            
            Color color = new Color32(80, 80, 80, 255);
            if (crators < 0)
                color = new Color32(50, 50, 50, 255);

            return new GeneratorData(crators + details, color);
        }

        public void Init(long seed)
        {
            noise = new OpenSimplexNoise(seed);
        }
    }
}
