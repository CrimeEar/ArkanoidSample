using UnityEngine;

public class BallMovement
{
    private const float MIN_VERTICAL_ANGLE_COLLISION = 15f;

    private float _moveSpeed;

    private bool _isMoveStarted;

    private Vector2 _velocity;
    private Vector2 _previousPosition;
    private Vector2 _newPosition;

    private BallObject _ballObject;
    private PaddleObject _paddle;
    private BordersObject _borders;

    private float _radius => _ballObject.Radius;
    private CircleBaseObject[] AllCircles => _ballObject.AllCircles;
    public bool IsMoveStarted => _isMoveStarted;

    public BallMovement(BallObject ballObject, BordersObject bordersObject, PaddleObject paddle, Vector2 startVelocity, float startSpeed)
    {
        _ballObject = ballObject;
        _paddle = paddle;
        _borders = bordersObject;
        _velocity = startVelocity;
        _moveSpeed = startSpeed;

        _previousPosition = (Vector2)ballObject.transform.position;
        _newPosition = _previousPosition;
    }

    public void CustomUpdate()
    {
        if (!_isMoveStarted)
        {
            _newPosition = _paddle.Position + (_radius + _paddle.Radius) * Vector2.up;
        }
        else
        {
            _previousPosition = _newPosition;
            _newPosition +=  _moveSpeed * Time.deltaTime * _velocity.normalized;

            HandleBorderCollisions();
            HandleCircleCollisions();
            HandlePaddleCollision(_paddle);
        }
        
        _ballObject.transform.position = (Vector3)_newPosition;
    }
    public void StarMove(bool isStartMove)
    {
        _isMoveStarted = isStartMove;
    }
    private void HandlePaddleCollision(PaddleObject paddle)
    {
        if (HandleCapsuleCollision(paddle.Position, paddle.Radius, paddle.Height))
        {
            AddPaddleVelocityDirection(paddle.MemoryVelocity);
            SoundSystem.Instance.PlaySound(AudioName.Show, 0.7f);
            paddle.OnCollided();
        }
    }
    private void HandleBorderCollisions()
    {
        foreach (Line border in _borders.BorderLines)
        {
            if (CheckIntersection(_previousPosition, _newPosition, border.StartLinePoint.position, border.EndLinePoint.position, out Vector2 intersection))
            {
                if(border == _borders.BorderLines[3])
                {
                    _isMoveStarted = false;
                    _velocity = Vector2.up;
                    _ballObject.HealthSystem.OnDeath();
                }
                if (!border.IsTrigger)
                {
                    Vector2 borderNormal = -GetLineNormal(border.StartLinePoint.position, border.EndLinePoint.position);

                    _velocity = Vector2.Reflect(_velocity, borderNormal);
                    _newPosition = _previousPosition + _velocity * Time.deltaTime;
                    _ballObject.transform.position = (Vector3)_newPosition;

                    SoundSystem.Instance.PlaySound(AudioName.Show, 0.7f); // TEMP

                    break;
                }
            }
        }
    }
    private void HandleCircleCollisions()
    {
        foreach (CircleBaseObject circle in AllCircles)
        {
            if (circle == _ballObject || circle.IsDestroyed) continue;

            float circleRadius = circle.Radius;
            float radiusSum = _radius / 2f + circleRadius;

            Vector2 circlePosition = circle.transform.position;
            Vector2 directionToCircle = circlePosition - _newPosition;

            float distanceToCircle = directionToCircle.magnitude;

            if (distanceToCircle < radiusSum)
            {
                Vector2 normal = directionToCircle.normalized;
                float overlap = radiusSum - distanceToCircle;

                _newPosition -= normal * overlap;
                _velocity = Vector2.Reflect(_velocity, normal);

                circle.PopIt();

                float pitch = 0.75f + 0.5f * (1f - circle.Radius / 1.5f); // TEMP
                SoundSystem.Instance.PlaySound(AudioName.Pop, pitch);

                ParticlePoolSystem.Instance.PlayParticle(circle.transform.position, circle.GizmoColor);

                _ballObject.BallPopResultantContainer.OnPopCircle(circle.transform.position);
            }
        }
    }

    private bool HandleCapsuleCollision(Vector2 capsulePosition, float capsuleRadius, float capsuleHeight)
    {
        float halfHeight = capsuleHeight / 2;
        Vector2 leftCapCenter = new Vector2(capsulePosition.x - halfHeight, capsulePosition.y);
        Vector2 rightCapCenter = new Vector2(capsulePosition.x + halfHeight, capsulePosition.y);

        Vector2 ballPosition = _newPosition;
        Vector2 movementDirection = _velocity.normalized;

        Vector2 rectNormal = GetLineNormal(leftCapCenter, rightCapCenter);

        if (CheckIntersection(_previousPosition, _newPosition, capsuleRadius * Vector2.left + leftCapCenter + rectNormal * _paddle.Radius, capsuleRadius * Vector2.right + rightCapCenter + rectNormal * _paddle.Radius, out Vector2 intersection))
        {
            float hitOffset = (intersection.x - _paddle.Position.x) / (_paddle.Height * 0.5f);

            hitOffset = Mathf.Clamp(hitOffset, -1f, 1f);

            float maxBounceAngle = 45f * Mathf.Deg2Rad;
            float bounceAngle = hitOffset * maxBounceAngle;

            Vector2 newDirection = new Vector2(Mathf.Sin(bounceAngle), Mathf.Cos(bounceAngle));
            newDirection = newDirection.normalized;

            _velocity = newDirection * _velocity.magnitude;

            _newPosition = intersection + _velocity * Time.deltaTime;

            return true;
        }

        /*Vector2 toLeftCap = ballPosition - leftCapCenter;
        if (toLeftCap.magnitude <= _radius / 2f + capsuleRadius)
        {
            Vector2 normal = toLeftCap.normalized;
            float overlap = (_radius / 2f + capsuleRadius) - toLeftCap.magnitude;

            _newPosition = ballPosition + normal * overlap;
            _velocity = Vector2.Reflect(_velocity, normal);

            return true;
        }

        Vector2 toRightCap = ballPosition - rightCapCenter;
        if (toRightCap.magnitude <= _radius / 2f + capsuleRadius)
        {
            Vector2 normal = toRightCap.normalized;
            float overlap = (_radius / 2f + capsuleRadius) - toRightCap.magnitude;

            _newPosition = ballPosition + normal * overlap;
            _velocity = Vector2.Reflect(_velocity, normal);

            return true;
        }*/

        return false;
    }

    private bool CheckIntersection(Vector2 p1, Vector2 p2, Vector2 q1, Vector2 q2, out Vector2 intersection)
    {
        intersection = Vector2.zero;

        Vector2 r = p2 - p1;
        Vector2 s = q2 - q1;
        float rxs = r.x * s.y - r.y * s.x;
        float qpxr = (q1 - p1).x * r.y - (q1 - p1).y * r.x;

        if (Mathf.Approximately(rxs, 0f) && Mathf.Approximately(qpxr, 0f))
            return false;

        if (Mathf.Approximately(rxs, 0f) && !Mathf.Approximately(qpxr, 0f))
            return false;

        float t = ((q1 - p1).x * s.y - (q1 - p1).y * s.x) / rxs;
        float u = ((q1 - p1).x * r.y - (q1 - p1).y * r.x) / rxs;

        if (t >= 0f && t <= 1f && u >= 0f && u <= 1f)
        {
            intersection = p1 + t * r;
            return true;
        }

        return false;
    }

    private Vector2 GetLineNormal(Vector2 start, Vector2 end)
    {
        Vector2 line = end - start;
        Vector2 normal = new Vector2(-line.y, line.x).normalized;
        Vector2 toBall = _newPosition - start;

        if (Vector2.Dot(toBall, normal) < 0)
        {
            normal = -normal;
        }

        return normal;
    }


    private void AddPaddleVelocityDirection(Vector2 paddleVelocity)
    {
        if (paddleVelocity.magnitude < _velocity.magnitude)
        {
            return;
        }

        Vector2 resultantVelocity = _velocity + paddleVelocity.normalized * _velocity.magnitude;

        _velocity = resultantVelocity.normalized * _velocity.magnitude;

        ClampBallAngle();
    }

    private void ClampBallAngle()
    {
        float angle = Vector2.Angle(_velocity, Vector2.right);

        if (angle < MIN_VERTICAL_ANGLE_COLLISION || angle > 180f - MIN_VERTICAL_ANGLE_COLLISION)
        {
            float sign = Mathf.Sign(_velocity.y);

            float clampedAngle = MIN_VERTICAL_ANGLE_COLLISION * Mathf.Deg2Rad;

            _velocity = new Vector2(_velocity.magnitude * Mathf.Cos(clampedAngle), sign * _velocity.magnitude * Mathf.Sin(clampedAngle));
        }
    }
}
