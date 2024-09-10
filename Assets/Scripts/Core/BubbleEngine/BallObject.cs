using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class BallObject : CircleBaseObject
{
    [SerializeField] private float _startSpeed;
    [SerializeField] private BubblesHandler _bubblesHandler;
    [SerializeField] private BordersObject _borders;
    [SerializeField] private PaddleObject _paddle;

    private Vector2 _velocity;

    private CircleBaseObject[] _allCircles;
    private BallMovement _ballMovement;
    private CollisionEventContainer _collisionEventContainer;
    private BubblePopResultantContainer _bubblePopResultantContainer;
    private HealthSystem _healthSystem;

    public HealthSystem HealthSystem => _healthSystem;
    public BubblePopResultantContainer BallPopResultantContainer => _bubblePopResultantContainer;
    public CollisionEventContainer CollisionEventContainer => _collisionEventContainer;
    public CircleBaseObject[] AllCircles => _allCircles;
    public bool IsMove => _ballMovement.IsMoveStarted;

    public void Init(CircleBaseObject[] allCircles, CollisionEventContainer collisionEventContainer, BubblePopResultantContainer bubblePopResultantContainer, HealthSystem healthSystem)
    {
        _bubblePopResultantContainer = bubblePopResultantContainer;
        _collisionEventContainer = collisionEventContainer;
        _healthSystem = healthSystem;
        _allCircles = allCircles;
        _velocity = Vector2.up;
        _ballMovement = new BallMovement(this, _borders, _paddle, _velocity, _startSpeed);

        InitCircleMeshGenerator(_bubblesHandler);
    }

    public void CustomUpdate()
    {
        _ballMovement.CustomUpdate();
        _circleMeshGenerator.CustomUpdate();
    }
    public void StartMove()
    {
        _velocity = Vector2.up;
        _ballMovement.StarMove(true);
    }

    protected override CircleBaseObject[] GetAllCircles()
    {
        return _allCircles;
    }

#if UNITY_EDITOR
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }
#endif
}
