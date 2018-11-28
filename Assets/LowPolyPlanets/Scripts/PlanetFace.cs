using Assets.Generators;
using System.Collections.Generic;
using UnityEngine;

public class PlanetFace : MonoBehaviour
{
    private Planet planet;

    private Vector3 localUp;
    private Vector3 localRight;
    private Vector3 localForward;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    public void Init(Vector3 localUp, Planet planet)
    {
        this.planet = planet;
        this.localUp = localUp;
        localRight = new Vector3(localUp.y, localUp.z, localUp.x);
        localForward = Vector3.Cross(localUp, localRight);

        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = planet.planetMaterial;
    }
    
    public void ConstructMesh()
    {
        int resolution = planet.resolution;
        Vector3[] points = new Vector3[resolution * resolution];
        GeneratorData[] data = new GeneratorData[resolution * resolution];

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = x + y * resolution;

                Vector2 normalized = new Vector2(x, y) / (resolution - 1);
                Vector3 pointOnUnitCube = localUp + (normalized.x - 0.5f) * 2 * localRight + (normalized.y - 0.5f) * 2 * localForward;
                float x2 = pointOnUnitCube.x * pointOnUnitCube.x;
                float y2 = pointOnUnitCube.y * pointOnUnitCube.y;
                float z2 = pointOnUnitCube.z * pointOnUnitCube.z;
                Vector3 point;
                point.x = pointOnUnitCube.x * Mathf.Sqrt(1f - y2 / 2f - z2 / 2f + y2 * z2 / 3f);
                point.y = pointOnUnitCube.y * Mathf.Sqrt(1f - x2 / 2f - z2 / 2f + x2 * z2 / 3f);
                point.z = pointOnUnitCube.z * Mathf.Sqrt(1f - x2 / 2f - y2 / 2f + x2 * y2 / 3f);

                points[i] = point / 2f;
            }
        }

        List<Vector3> verticies = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Color> colors = new List<Color>();

        for (int i = 0; i < points.Length; i++)
        {
            data[i] = planet.generator.GetData(points[i]);
            points[i] += points[i] * data[i].noise;
            points[i] *= planet.size;
        }
        
        for (int y = 0; y < resolution - 1; y++)
        {
            for (int x = 0; x < resolution - 1; x++)
            {
                Vector3 pointA = points[x + y * resolution];
                Vector3 pointB = points[(x + 1) + y * resolution];
                Vector3 pointC = points[(x + 1) + (y + 1) * resolution];
                Vector3 pointD = points[x + (y + 1) * resolution];

                Color colorA = data[x + y * resolution].color;
                Color colorB = data[(x + 1) + y * resolution].color;
                Color colorC = data[(x + 1) + (y + 1) * resolution].color;
                Color colorD = data[x + (y + 1) * resolution].color;

                if ((x + y) % 2 == 0)
                {
                    AddTriangle(pointA, pointB, pointC, colorA, colorB, colorC, verticies, triangles, colors);
                    AddTriangle(pointC, pointD, pointA, colorC, colorD, colorA, verticies, triangles, colors);
                }
                else
                {
                    AddTriangle(pointA, pointB, pointD, colorA, colorB, colorD, verticies, triangles, colors);
                    AddTriangle(pointB, pointC, pointD, colorB, colorC, colorD, verticies, triangles, colors);
                }
            }
        }

        Mesh mesh = new Mesh();
        mesh.vertices = verticies.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.colors = colors.ToArray();
        mesh.RecalculateNormals();
        meshFilter.sharedMesh = mesh;
    }
    
    private void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3, Color c1, Color c2, Color c3, List<Vector3> verticies, List<int> triangles, List<Color> colors)
    {
        int vertexCount = verticies.Count;

        colors.Add(c1);
        colors.Add(c2);
        colors.Add(c3);

        verticies.Add(v1);
        verticies.Add(v2);
        verticies.Add(v3);

        triangles.Add(vertexCount);
        triangles.Add(vertexCount + 1);
        triangles.Add(vertexCount + 2);
    }
}
