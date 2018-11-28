using UnityEngine;

namespace Assets.Generators
{
    public struct GeneratorData
    {
        public float noise;
        public Color color;

        public GeneratorData(float noise, Color color)
        {
            this.noise = noise;
            this.color = color;
        }
    }
}
