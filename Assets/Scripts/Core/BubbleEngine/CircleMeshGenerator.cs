using System.Collections.Generic;
using UnityEngine;

public class CircleMeshGenerator
{
    public float radius = 0.01f;
    public int segments = 36;
    public Color gizmoColor = Color.white;
    public float pumpSpeed = 1f;
    public Transform CurrentTransform;

    private MeshFilter meshFilter;
    private Mesh mesh;
    private Vector2[] baseVertices;
    private Vector3[] vertices;
    private Vector2[] uv;
    private int[] triangles;

    public CircleMeshGenerator(BubblesHandler bubbleHandler, CircleBaseObject circleObject, MeshFilter meshFilter, CircleBaseObject[] allCircles)
    {
        this.meshFilter = meshFilter;

        radius = circleObject.Radius;
        CurrentTransform = circleObject.transform;

        if (mesh == null)
        {
            mesh = new Mesh();
            mesh.MarkDynamic();
        }
        meshFilter.mesh = mesh;

        GenerateCircle();
    }
    public void CustomUpdate()
    {
        
    }

    private void GenerateCircle()
    {
        if (baseVertices == null || baseVertices.Length != segments + 1)
        {
            baseVertices = new Vector2[segments + 1];
            vertices = new Vector3[segments + 1];
            uv = new Vector2[segments + 1];
            triangles = new int[segments * 3];
        }

        baseVertices[0] = Vector2.zero;
        uv[0] = new Vector2(0.5f, 0.5f);

        float angleStep = 360f / segments;

        for (int i = 0; i < segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            baseVertices[i + 1] = new Vector2(x, y);
            vertices[i + 1] = new Vector3(x, y, 0);

            float u = (Mathf.Cos(angle) + 1f) / 2f;
            float v = (Mathf.Sin(angle) + 1f) / 2f;
            uv[i + 1] = new Vector2(u, v);

            if (i < segments - 1)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 2;
                triangles[i * 3 + 2] = i + 1;
            }
            else
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = 1;
                triangles[i * 3 + 2] = i + 1;
            }
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();
    }

    public void Hide()
    {
        radius = 0f;
        GenerateCircle();
    }
    public void RedrawMesh()
    {
        mesh.vertices = vertices;
    }
}