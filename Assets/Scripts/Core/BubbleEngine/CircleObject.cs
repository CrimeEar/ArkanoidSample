using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class CircleObject : CircleBaseObject
{
    [Header("Init Parameters")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _movementOffset = 0.15f;
    [SerializeField] private Vector2 _changePositionDelayRange;

    private RandomMovement _circleMovement;
    private BubblesHandler _bubblesHandler;
    private BubblePopResultantContainer _bubblePopResultantContainer;

    private float startDelay = 0f;
    private bool _isShowed;

    public float MoveSpeed => _moveSpeed;
    public RandomMovement CircleMovement => _circleMovement;

    public void Init(BubblesHandler bubblesHandler, Line startLine, BubblePopResultantContainer bubblePopResultantContainer)
    {
        _bubblePopResultantContainer = bubblePopResultantContainer;
        _bubblesHandler = bubblesHandler;

        InitCircleMeshGenerator(bubblesHandler);

        _circleMovement = new RandomMovement(bubblesHandler, this, Radius, MoveSpeed, _movementOffset, _changePositionDelayRange);
        Vector2 closestPointOnLine = GetClosestPointOnLineAndStartDelay(startLine, transform.position);
        _circleMovement.SetPosition(closestPointOnLine);
        _isDestroyed = false;
        _isShowed = false;
    }

    public void CustomUpdate(float timeT)
    {
        if (timeT < startDelay)
        {
            return;
        }
        if (!_isShowed)
        {
            _isShowed = true;
            float pitch = 0.75f + 0.5f * (1f - Radius / 1.5f); // TEMP
            SoundSystem.Instance.PlaySound(AudioName.Show, pitch);
        }

        _circleMovement.CustomUpdate();
        _circleMeshGenerator.CustomUpdate();
    }
    public override void AddMoveVector(Vector3 vectorAdd)
    {
        base.AddMoveVector(vectorAdd);
        _circleMovement.AddMove(vectorAdd);
    }
    private Vector2 GetClosestPointOnLineAndStartDelay(Line line, Vector2 point)
    {
        Vector2 start = (Vector2)line.StartLinePoint.position;
        Vector2 end = (Vector2)line.EndLinePoint.position;
        Vector2 direction = end - start;
        Vector2 lineToPoint = point - start;

        float projection = Vector2.Dot(lineToPoint, direction.normalized);
        projection = Mathf.Clamp(projection, 0, direction.magnitude);

        Vector2 closestPoint = start + direction.normalized * projection;

        startDelay = (closestPoint - start).magnitude / (start - end).magnitude;

        return closestPoint;
    }

    protected override CircleBaseObject[] GetAllCircles()
    {
        return _bubblesHandler.AllBaseCircles;
    }

#if UNITY_EDITOR
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }
#endif
}
