using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class CircleObject : CircleBaseObject
{
    [Header("Init Parameters")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _movementOffset = 0.15f;
    [SerializeField] private Vector2 _changePositionDelayRange;

    private RandomMovement _circleMovement;

    private float startDelay = 0f;

    public float MoveSpeed => _moveSpeed;
    public RandomMovement CircleMovement => _circleMovement;

    public void Init(BubblesHandler bubblesHandler, Line startLine)
    {
        InitCircleMeshGenerator(bubblesHandler);

        _circleMovement = new RandomMovement(bubblesHandler, this, Radius, MoveSpeed, _movementOffset, _changePositionDelayRange);
        Vector2 closestPointOnLine = GetClosestPointOnLineAndStartDelay(startLine, transform.position);
        _circleMovement.SetPosition(closestPointOnLine);
        _isDestroyed = false;
    }

    public void CustomUpdate(float timeT)
    {
        if (timeT < startDelay)
        {
            return;
        }

        _circleMovement.CustomUpdate();
        _circleMeshGenerator.CustomUpdate();
    }
    public void PopIt()
    {
        _isDestroyed = true;
        _circleMeshGenerator.Hide();
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

    protected override CircleObject[] GetAllCircles()
    {
        return new CircleObject[] { this };
    }

#if UNITY_EDITOR
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }
#endif
}
