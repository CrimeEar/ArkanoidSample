using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class BallObject : CircleBaseObject
{
    [SerializeField] private float _startSpeed;
    [SerializeField] private BubblesHandler _bubblesHandler;
    [SerializeField] private BordersObject _borders;
    [SerializeField] private PaddleObject _paddle;

    private Vector2 _velocity;

    private CircleObject[] _allCircles;
    private BallMovement _ballMovement;
    private CollisionEventContainer _collisionEventContainer;

    public CollisionEventContainer CollisionEventContainer => _collisionEventContainer;
    public CircleObject[] AllCircles => _allCircles;

    public void Init(CircleObject[] allCircles, CollisionEventContainer collisionEventContainer)
    {
        _collisionEventContainer = collisionEventContainer;
        _allCircles = allCircles;
        _velocity = Vector2.up;
        _ballMovement = new BallMovement(this, _borders, _paddle, _velocity, _startSpeed);

        InitCircleMeshGenerator(_bubblesHandler);
    }

    public void CustomUpdate()
    {
        _ballMovement.CustomUpdate();
    }
    public void StartMove()
    {
        _ballMovement.StarMove(true);
    }

    protected override CircleObject[] GetAllCircles()
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
