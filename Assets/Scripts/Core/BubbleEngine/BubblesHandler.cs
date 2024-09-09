using System;
using UnityEngine;

public class BubblesHandler : MonoBehaviour
{
    [SerializeField] private Line _startLine;
    [SerializeField] private float _startDuration = 0.8f;

    private CircleObject[] _allCircles;
    private int _currentBubbleToPop;
    private float startTime = -1f;

    public CircleObject[] AllCircles => _allCircles;

    public void Init()
    {
        _allCircles = GetComponentsInChildren<CircleObject>();

        SetToStartPositions();

        startTime = Time.time;
    }
    private void SetToStartPositions()
    {
        for (int i = 0; i < _allCircles.Length; i++)
        {
            _allCircles[i].Init(this, _startLine);
        }
    }

    public void CustomUpdate()
    {
        if(startTime < 0f)
        {
            return;
        }

        for(int i = _currentBubbleToPop;  i < _allCircles.Length; i++)
        {
            _allCircles[i].CustomUpdate((Time.time - startTime) / _startDuration);
        }
    }

    public void PopAllBubbles()
    {
        foreach(var circle in _allCircles)
        {
            if (!circle.IsDestroyed)
            {
                circle.PopIt();
            }
        }
    }
}

[Serializable]
public class Line : ICollisioned
{
    public Transform StartLinePoint;
    public Transform EndLinePoint;

    public bool IsTrigger;
}
