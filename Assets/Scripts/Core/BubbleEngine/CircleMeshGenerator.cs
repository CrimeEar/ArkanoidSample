using UnityEngine;

public class CircleMeshGenerator
{
    public float Radius = 1f;
    public int Segments = 36;
    public Color GizmoColor = Color.white;
    public Transform CurrentTransform;

    private MeshFilter _meshFilter;
    private Mesh _mesh;

    private Vector2[] _baseVertices;
    private Vector3[] _vertices;
    private Vector2[] _uv;
    private int[] _triangles;

    public CircleMeshGenerator(BubblesHandler bubbleHandler, CircleBaseObject circleObject, MeshFilter meshFilter, CircleObject[] allCircles)
    {
        this._meshFilter = meshFilter;
        
        this.Radius = circleObject.Radius;
        this.CurrentTransform = circleObject.transform;

        if (_mesh == null)
        {
            _mesh = new Mesh();
            _mesh.MarkDynamic();
        }
        meshFilter.mesh = _mesh;

        GenerateCircle();
    }
    public void Hide()
    {
        Radius = 0f;
        GenerateCircle();
    }
    public void CustomUpdate()
    {
        // redraw mesh if need to
    }

    private void GenerateCircle()
    {
        if (_baseVertices == null || _baseVertices.Length != Segments + 1)
        {
            _baseVertices = new Vector2[Segments + 1];
            _vertices = new Vector3[Segments + 1];
            _uv = new Vector2[Segments + 1];
            _triangles = new int[Segments * 3];
        }

        _baseVertices[0] = Vector2.zero;
        _uv[0] = new Vector2(0.5f, 0.5f);

        float angleStep = 360f / Segments;

        for (int i = 0; i < Segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            float x = Mathf.Cos(angle) * Radius;
            float y = Mathf.Sin(angle) * Radius;
            _baseVertices[i + 1] = new Vector2(x, y);
            _vertices[i + 1] = new Vector3(x, y, 0);

            float u = (Mathf.Cos(angle) + 1f) / 2f;
            float v = (Mathf.Sin(angle) + 1f) / 2f;
            _uv[i + 1] = new Vector2(u, v);

            if (i < Segments - 1)
            {
                _triangles[i * 3] = 0;
                _triangles[i * 3 + 1] = i + 2;
                _triangles[i * 3 + 2] = i + 1;
            }
            else
            {
                _triangles[i * 3] = 0;
                _triangles[i * 3 + 1] = 1;
                _triangles[i * 3 + 2] = i + 1;
            }
        }

        _mesh.Clear();
        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;
        _mesh.uv = _uv;
        _mesh.RecalculateNormals();
    }
}
