using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class PaddleObject : MonoBehaviour, ICollisioned
{
    [SerializeField] private Color _gizmoColor;

    [Header("Init Parameters")]
    [SerializeField] private float _radius;
    [SerializeField] private float _height;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private bool _isVertical;

    private CapsuleMeshGenerator _capsuleMeshGenerator;
    private BordersObject _borderObject;
    private InputSystem _inputSystem;
    private PaddleMovement _paddleMovement;

    public float Radius => _radius;
    public float Height => _height;
    public float MoveSpeed => _moveSpeed;
    public Vector2 Velocity => _paddleMovement.Velocity;
    public Vector2 MemoryVelocity => _paddleMovement.MemoryVelocity;
    public Vector2 Position => _paddleMovement.Position;
    public CapsuleMeshGenerator CapsuleMeshGenerator => _capsuleMeshGenerator;
    public PaddleMovement PaddleMovement => _paddleMovement;

    private void OnDestroy()
    {
        if(_paddleMovement != null)
        {
            _paddleMovement.Release();
        }
    }
    public void Init(InputSystem inputSystem, BordersObject borders)
    {
        _inputSystem = inputSystem;
        _borderObject = borders;

        MeshFilter meshFilter = GetComponent<MeshFilter>();

        _paddleMovement = new PaddleMovement(_inputSystem, this, _borderObject);
        _capsuleMeshGenerator = new CapsuleMeshGenerator(meshFilter, _radius, _height, 36, _isVertical);
    }

    public void CustomUpdate()
    {
        _paddleMovement.CustomUpdate();
        _capsuleMeshGenerator.CustomUpdate();
    }
    public void OnCollided()
    {
        _paddleMovement.OnCollide();
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            Gizmos.color = _gizmoColor;
            DrawGizmoCapsule(transform.position, Radius, Height);
        }
    }

    private void DrawGizmoCapsule(Vector3 position, float radius, float height)
    {
        Vector3 vectorHeight = Vector3.right;
        Vector3 vectorRadius = Vector3.up;
        if (_isVertical)
        {
            vectorHeight = Vector3.up;
            vectorRadius = Vector3.right;
        }
        Gizmos.DrawWireSphere(position + vectorHeight * (height / 2 - radius), radius);
        Gizmos.DrawWireSphere(position - vectorHeight * (height / 2 - radius), radius);

        Gizmos.DrawLine(position + vectorRadius * radius + vectorHeight * (height / 2 - radius),
                        position + vectorRadius * radius - vectorHeight * (height / 2 - radius));
        Gizmos.DrawLine(position - vectorRadius * radius + vectorHeight * (height / 2 - radius),
                        position - vectorRadius * radius - vectorHeight * (height / 2 - radius));
    }
#endif
}
