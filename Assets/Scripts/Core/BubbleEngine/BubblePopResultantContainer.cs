using UnityEngine;

public class BubblePopResultantContainer
{
    private Vector3 _resultant;
    private float _resultantForceValue;

    private int _poppedCircles;

    public BubblePopResultantContainer(Vector3 resultant, float resultantForceValue)
    {
        _resultant = resultant;
        _resultantForceValue = resultantForceValue;
    }

    public int PoppedCircles { get { return _poppedCircles; } }
    public Vector3 Resultant { get { return _resultant; } }
    public float ResultantForceValue { get { return _resultantForceValue; } }

    public void OnPopCircle(Vector3 circlePosition)
    {
        _poppedCircles++;
        if(_resultant == Vector3.zero)
        {
            _resultant = circlePosition;
        }
        else
        {
            _resultant += circlePosition;
            _resultant /= 2f;
        }
    }
}
