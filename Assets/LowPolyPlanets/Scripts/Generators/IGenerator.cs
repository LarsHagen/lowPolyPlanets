using UnityEngine;

namespace Assets.Generators
{
    public interface IGenerator
    {
        void Init(long seed);
        GeneratorData GetData(Vector3 position);
    }
}
