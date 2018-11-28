using Assets.Generators;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [System.NonSerialized] public int seed;
    [System.NonSerialized] public int resolution;
    [System.NonSerialized] public float size;
    [System.NonSerialized] public IGenerator generator;
    [System.NonSerialized] public Material planetMaterial;
    [System.NonSerialized] public PlanetFace[] planetFaces;
    [System.NonSerialized] public Transform water;
    
    public void Initialize(int seed, int resolution, float size, bool hasWater, IGenerator generator, Material planetMaterial, Material waterMaterial)
    {
        this.seed = seed;
        this.resolution = resolution;
        this.size = size;
        this.planetMaterial = planetMaterial;

        this.generator = generator;
        generator.Init(seed);

        if (hasWater)
        {
            water = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
            water.gameObject.name = "Ocean";
            water.GetComponent<MeshRenderer>().sharedMaterial = waterMaterial;
            water.SetParent(transform, false);
            water.localScale = Vector3.one * size;
        }

        planetFaces = new PlanetFace[6];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; i++)
        {
            GameObject planetFace = new GameObject("PlanetFace");
            planetFace.transform.SetParent(transform, false);
            planetFaces[i] = planetFace.AddComponent<PlanetFace>();
            planetFaces[i].Init(directions[i], this);
        }
    }

    public void GenerateMesh()
    {
        foreach (var planetFace in planetFaces)
        {
            planetFace.ConstructMesh();
        }
    }
}
