using Assets.Generators;
using UnityEngine;

public class Example : MonoBehaviour
{
    public Material waterMaterial;
    public Material planetMaterial;

    private Planet planet;

    private void Update()
    {
        if (planet != null)
        {
            planet.transform.eulerAngles = new Vector3(0, Time.time * 10f / planet.size, 0);
        }
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Spawn earthlike"))
        {
            SpawnPlanet("Earth like", 30, 2, true, new EarthLikeGenerator());
        }
        if (GUILayout.Button("Spawn moon"))
        {
            SpawnPlanet("Moon", 15, 1, false, new MoonLikeGenerator());
        }
        if (GUILayout.Button("Spawn gas giant"))
        {
            SpawnPlanet("Gas giant", 35, 5, false, new GasGiantGenerator());
        }
    }

    private void SpawnPlanet(string name, int resolution, int size, bool water, IGenerator generator)
    {
        if (planet != null)
        {
            Destroy(planet.gameObject);
        }

        planet = new GameObject(name).AddComponent<Planet>();
        planet.transform.position = new Vector3(0, 0, 0);
        planet.Initialize(Random.Range(int.MinValue, int.MaxValue), resolution, size, water, generator, planetMaterial, waterMaterial);
        planet.GenerateMesh();
    }
}
