using System;

public class CollisionEventContainer
{
    public event Action<Line> OnBorderCollidedEvent;
    public event Action<CircleObject> OnCircleObjectEvent;

    public void OnBorderCollided(Line border)
    {
        OnBorderCollidedEvent?.Invoke(border);
    }
    public void OnCircleCollided(CircleObject circle)
    {
        OnCircleObjectEvent?.Invoke(circle);
    }
}
