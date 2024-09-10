using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public abstract class CircleBaseObject : MonoBehaviour, ICollisioned
{
    [SerializeField] protected float _radius;
    [SerializeField] protected Color _gizmoColor;

    protected bool _isDestroyed;
    public bool IsDestroyed => _isDestroyed;
    public float Radius => _radius;
    public Color GizmoColor => _gizmoColor;

    protected CircleMeshGenerator _circleMeshGenerator;

    public CircleMeshGenerator CircleMeshGenerator => _circleMeshGenerator;

    protected virtual void InitCircleMeshGenerator(BubblesHandler bubblesHandler)
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        _circleMeshGenerator = new CircleMeshGenerator(bubblesHandler, this, meshFilter, GetAllCircles());
    }

    protected abstract CircleBaseObject[] GetAllCircles();
    public virtual void PopIt()
    {
        _isDestroyed = true;
        _circleMeshGenerator.Hide();
    }
    public virtual void AddMoveVector(Vector3 addVector)
    {

    }

#if UNITY_EDITOR
    protected virtual void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            Gizmos.color = _gizmoColor;
            Gizmos.DrawSphere(transform.position, Radius);
        }
    }
#endif
}