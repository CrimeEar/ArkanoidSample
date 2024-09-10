using System;
using UnityEngine;

public class BubblesHandler : MonoBehaviour
{
    [SerializeField] private Line _startLine;
    [SerializeField] private float _startDuration = 0.8f;

    private CircleObject[] _alllCircles;
    private CircleBaseObject[] _allBaseCircles;
    BubblePopResultantContainer _bubblePopResultantContainer;

    private int _currentBubbleToPop;
    private float startTime = -1f;

    public CircleObject[] AllCircles {  get { return _alllCircles; } }
    public CircleBaseObject[] AllBaseCircles { get { return _allBaseCircles; } }
    public BubblePopResultantContainer BubblePopResultantContainer { get {  return _bubblePopResultantContainer; } }

    public void Init(BubblePopResultantContainer bubblePopResultantContainer = null, BallObject ballObject = null)
    {
        _bubblePopResultantContainer = bubblePopResultantContainer;
        _alllCircles = GetComponentsInChildren<CircleObject>();

        InitAllBaseCircles(ballObject);

        SetToStartPositions();

        startTime = Time.time;
    }
    private void SetToStartPositions()
    {
        for (int i = 0; i < _alllCircles.Length; i++)
        {
            _alllCircles[i].Init(this, _startLine, _bubblePopResultantContainer);
        }
    }
    private void InitAllBaseCircles(BallObject ballObject)
    {
        int count = ballObject == null? _alllCircles.Length : _alllCircles.Length + 1;
        _allBaseCircles = new CircleBaseObject[count];

        for(int i = 0; i < _alllCircles.Length; i++)
        {
            _allBaseCircles[i] = _alllCircles[i];
        }

        if(ballObject != null)
        {
            _allBaseCircles[count - 1] = ballObject;
        }
    }
    public void CustomUpdate()
    {
        if(startTime < 0f)
        {
            return;
        }

        for(int i = _currentBubbleToPop;  i < _alllCircles.Length; i++)
        {
            _alllCircles[i].CustomUpdate((Time.time - startTime) / _startDuration);
        }
    }

    public void PopAllBubbles()
    {
        foreach(var circle in _alllCircles)
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
