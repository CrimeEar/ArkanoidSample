using UnityEngine;

public class CapsuleMeshGenerator
{
    public float Radius;
    public float Length;
    public int Segments;
    public Color GizmoColor = Color.white;
    public bool IsVertical = true;
    public Transform CurrentTransform;

    private MeshFilter _meshFilter;
    private Mesh _mesh;

    private Vector2[] _baseVertices;
    private Vector3[] _vertices;
    private Vector2[] _uv;
    private int[] _triangles;

    public CapsuleMeshGenerator(MeshFilter meshFilter, float radius, float length, int segments, bool isVertical)
    {
        this._meshFilter = meshFilter;
        this.Radius = radius;
        this.Length = length;
        this.Segments = segments;
        this.IsVertical = isVertical;

        if (_mesh == null)
        {
            _mesh = new Mesh();
            _mesh.MarkDynamic();
        }
        meshFilter.mesh = _mesh;

        GenerateCapsule();
    }

    private void GenerateCapsule()
    {
        int halfSegments = Segments / 2;
        int totalVertices = 2 * (halfSegments + 1) + 4;
        int totalTriangles = (4 * halfSegments) + 2; 

        _baseVertices = new Vector2[totalVertices];
        _vertices = new Vector3[totalVertices];
        _uv = new Vector2[totalVertices];
        _triangles = new int[totalTriangles * 3];

        float angleStep = 180f / halfSegments;
        float bodyLength = Length - 2 * Radius;

        _uv[0] = new Vector2(0.5f, 0.5f);
        for (int i = 0; i <= halfSegments; i++)
        {
            float angle = (i * angleStep - (IsVertical ? 0f : 90f)) * Mathf.Deg2Rad;
            float x = Mathf.Cos(angle) * Radius;
            float y = Mathf.Sin(angle) * Radius;

            if (IsVertical)
            {
                _baseVertices[i] = new Vector2(x, y + bodyLength / 2f );
            }
            else
            {
                _baseVertices[i] = new Vector2(x + bodyLength / 2f, y);
            }

            _vertices[i] = _baseVertices[i];
            float u = (Mathf.Cos(angle - 90f) + 1f) / 2f;
            float v = (Mathf.Sin(angle - 90f) + 1f) / 2f;
            _uv[i] = new Vector2(u, v);
        }

        for (int i = 0; i <= halfSegments; i++)
        {
            float angle = (i * angleStep - (IsVertical ? 0f : 90f)) * Mathf.Deg2Rad;
            float x = Mathf.Cos(angle) * Radius;
            float y = Mathf.Sin(angle) * Radius;

            if (IsVertical)
            {
                _baseVertices[halfSegments + 1 + i] = new Vector2(x, -y - bodyLength / 2f);
            }
            else
            {
                _baseVertices[halfSegments + 1 + i] = new Vector2(-x - bodyLength / 2f, y);
            }

            _vertices[halfSegments + 1 + i] = _baseVertices[halfSegments + 1 + i];
            float u = (Mathf.Cos(angle -90f) + 1f) / 2f;
            float v = (Mathf.Sin(angle -90f) + 1f) / 2f;
            _uv[halfSegments + i + 1] = new Vector2(u, v);
        }

        if (IsVertical)
        {
            _vertices[totalVertices - 4] = new Vector2(-Radius, bodyLength / 2f);
            _vertices[totalVertices - 3] = new Vector2(Radius, bodyLength / 2f);
            _vertices[totalVertices - 2] = new Vector2(Radius, -bodyLength / 2f);
            _vertices[totalVertices - 1] = new Vector2(-Radius, -bodyLength / 2f);
        }
        else
        {
            _vertices[totalVertices - 4] = new Vector2(bodyLength / 2f, -Radius);
            _vertices[totalVertices - 3] = new Vector2(bodyLength / 2f, Radius);
            _vertices[totalVertices - 2] = new Vector2(-bodyLength / 2f, Radius);
            _vertices[totalVertices - 1] = new Vector2(-bodyLength / 2f, -Radius);
        }

        _uv[totalVertices - 4] = new Vector2(0, 0.45f);
        _uv[totalVertices - 3] = new Vector2(1, 0.65f);
        _uv[totalVertices - 2] = new Vector2(1, 0.45f);
        _uv[totalVertices - 1] = new Vector2(0, 0.65f);

        int t = 0;
        for (int i = 0; i < halfSegments; i++)
        {
            _triangles[t++] = totalVertices - 4; 
            _triangles[t++] = i + 1;
            _triangles[t++] = i;
        }

        for (int i = 0; i < halfSegments; i++)
        {
            _triangles[t++] = halfSegments + 1 + i;
            _triangles[t++] = halfSegments + 2 + i;
            _triangles[t++] = totalVertices - 1; 
        }

        if (IsVertical)
        {
            _triangles[t++] = totalVertices - 4; 
            _triangles[t++] = totalVertices - 3; 
            _triangles[t++] = totalVertices - 2; 

            _triangles[t++] = totalVertices - 4;
            _triangles[t++] = totalVertices - 2; 
            _triangles[t++] = totalVertices - 1; 
        }
        else
        {
            _triangles[t++] = totalVertices - 2; 
            _triangles[t++] = totalVertices - 3; 
            _triangles[t++] = totalVertices - 4; 

            _triangles[t++] = totalVertices - 1; 
            _triangles[t++] = totalVertices - 2; 
            _triangles[t++] = totalVertices - 4; 
        }
        
        _mesh.Clear();
        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;
        _mesh.uv = _uv;
        _mesh.RecalculateNormals();
    }

    public void CustomUpdate()
    {
        // Optional: Implement if mesh needs to be dynamically updated
    }
}
